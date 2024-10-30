using Bookstore.BL.Interfaces;
using Bookstore.BL.Dto.Auth;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace Bookstore.BL.Services
{
    public class AuthService : IAuthService
    {

        private readonly IConfiguration _config;
        public AuthService(IConfiguration config)
        {
            _config = config;
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
            throw new NotImplementedException("Данный метод не реализован");
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
    }
}
