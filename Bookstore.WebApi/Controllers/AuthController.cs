using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bookstore.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        // TODO: Дописать DTO, сделать валидацию
        [HttpPost("login")]
        public IActionResult Login(string userLogin)
        {
            if (userLogin == null || (userLogin != "admin" && userLogin != "user"))
            {
                return Unauthorized("Привет");
            }


            var token = GenerateJwtToken(userLogin);
            return Ok(new { token });

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

        // Добавить refreshToken
    }
}
