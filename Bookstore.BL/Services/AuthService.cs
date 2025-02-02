using Bookstore.BL.Interfaces;
using Bookstore.BL.Dto.Auth;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Bookstore.DAL.Interfaces;
using Bookstore.Shared.Exceptions;
using Bookstore.DAL.Entities;

namespace Bookstore.BL.Services
{
    public class AuthService : IAuthService
    {

        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRefreshTokenRepository _userRefreshTokenRepository;
        public AuthService(IConfiguration config,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUserRefreshTokenRepository refreshTokenRepository
            )
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRefreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthByLoginResponseDto> Login(AuthByLoginDto authByLoginDto)
        {
            var user = await _userRepository.GetByLoginAsync(authByLoginDto.Login);

            if (user == null)
            {
                throw new NotFoundException("Пользователя с данным login не найдено");
            }

            if (user.Password != authByLoginDto.Password)
            {
                throw new BadRequestException("Указан неверный пароль");
            }

            var role = await _roleRepository.GetByIdAsync(user.RoleId) ?? throw new BadRequestException("Недопустимая роль пользователя");

            user.Role = role;
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken(user);

            await _unitOfWork.BeginTransactionAsync();
            await _userRefreshTokenRepository.AddAsync(new UsersRefreshToken { RefreshToken = refreshToken });
            await _unitOfWork.CommitTransactionAsync();

            var token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

            return new AuthByLoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = token.ValidTo,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                RoleName = user.Role.Name
            };
        }

        public async Task<bool> Register(RegisterDto registerDto)
        {
            ValidateNewUser(registerDto);

            var existingUser = await _userRepository.GetByLoginAsync(registerDto.Login);

            if (existingUser != null)
            {
                throw new DuplicateException("Пользователь с таким логином уже существует");
            }

            var newUser = new User
            {
                Login = registerDto.Login,
                Email = registerDto.Email,
                FirstName = registerDto.Firstname,
                LastName = registerDto.Lastname,
                MiddleName = registerDto.Middlename,
                Password = registerDto.Password,
                RoleId = registerDto.RoleId
            };

            await _unitOfWork.BeginTransactionAsync();
            await _userRepository.AddAsync(newUser);
            await _unitOfWork.CommitTransactionAsync();

            return true;
        }

        public async Task<RefreshTokenResponseDto> RefreshToken(string refreshToken)
        {
            try
            {
                var tokenDetails = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken);
                var expiration = tokenDetails.ValidTo;

                var compareResult = DateTime.Compare(DateTime.Now, expiration);
                if (compareResult > 0)
                {
                    throw new BadRequestException("Время жизни токена истекло");
                }

                await _unitOfWork.BeginTransactionAsync();

                var existingToken = await _userRefreshTokenRepository.FindAsync(refreshToken);

                if (existingToken != null)
                {
                    var result = await ProcessRefreshToken(existingToken.RefreshToken);
                    _userRefreshTokenRepository.Delete(existingToken);
                    await _userRefreshTokenRepository.AddAsync(new UsersRefreshToken { RefreshToken = result.RefreshToken });

                    await _unitOfWork.CommitTransactionAsync();
                    return result;
                }
                else
                {
                    throw new BadRequestException("Токен не валиден");
                }
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

        }
        private string GenerateAccessToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Add roles to the claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (user.Role.Name == "admin")
            {
                claims.Append(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                claims.Append(new Claim(ClaimTypes.Role, "User"));
            }

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken(User user)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


            var refreshToken = new JwtSecurityToken(
               issuer: _config["Jwt:Issuer"],
               audience: _config["Jwt:Audience"],
               expires: DateTime.Now.AddDays(Convert.ToDouble(_config["Jwt:RefreshExpireDays"])),
               signingCredentials: credentials,
               claims: claims
           );


            return new JwtSecurityTokenHandler().WriteToken(refreshToken);
        }
        private void ValidateNewUser(RegisterDto registerDto)
        {
            Dictionary<string, string[]> errors = [];

            if (string.IsNullOrEmpty(registerDto.Login))
            {
                errors.Add("Login", ["Поле Login обязательно для заполнения"]);
            }

            if (string.IsNullOrEmpty(registerDto.Firstname))
            {
                errors.Add("Firstname", ["Поле Firstname обязательно для заполнения"]);
            }

            if (string.IsNullOrEmpty(registerDto.Lastname))
            {
                errors.Add("Lastname", ["Поле Lastname обязательно для заполнения"]);
            }

            if (string.IsNullOrEmpty(registerDto.Password))
            {
                errors.Add("Password", ["Поле Password обязательно для заполнения"]);
            }

            if (registerDto.RoleId == null || registerDto.RoleId == Guid.Empty)
            {
                errors.Add("RoleId", ["Поле RoleId обязательно для заполнения"]);
            }

            if (errors.Count > 0)
            {
                throw new BadRequestException(errors);
            }
        }


        private async Task<RefreshTokenResponseDto> ProcessRefreshToken(string refreshToken)
        {
            var tokenDetails = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken);
            var userLogin = tokenDetails.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value!;
            var user = await _userRepository.GetByLoginAsync(userLogin);

            if (user != null)
            {
                var role = await _roleRepository.GetByIdAsync(user.RoleId) ?? throw new BadRequestException("Недопустимая роль пользователя");
                user.Role = role;
                var newAccessToken = GenerateAccessToken(user);
                var newRefreshToken = GenerateRefreshToken(user);
                var expiresAt = new JwtSecurityTokenHandler().ReadJwtToken(newAccessToken).ValidTo;

                return new RefreshTokenResponseDto { RefreshToken = newRefreshToken, AccessToken = newAccessToken, ExpiresAt = expiresAt };
            }
            else
            {
                throw new NotFoundException("Соответствующий пользователь не найден");
            }

        }
    }
}
