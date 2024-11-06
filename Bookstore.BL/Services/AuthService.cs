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
        public AuthService(IConfiguration config, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        // TODO: implement
        public async Task<AuthByLoginResponseDto> Login(AuthByLoginDto authByLoginDto)
        {
            // Создать репозиторий User, реализовать метод для поиска в бд по логину.
            // Проверить соответствие паролей
            // Если совпадение найдено, вернуть GenerateJwtToken с соответствующей ролью
            return null;
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

        private string GenerateJwtToken(string userLogin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Add roles to the claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userLogin),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (userLogin == "admin")
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
    }
}
