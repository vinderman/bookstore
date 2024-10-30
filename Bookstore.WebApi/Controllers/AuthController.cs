using Bookstore.BL.Dto.Auth;
using Bookstore.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Bookstore.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;

        public AuthController(IConfiguration config, IAuthService authService)
        {
            _config = config;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthByLoginResponseDto>> Login([FromBody] AuthByLoginDto authByLoginDto)
        {
            try
            {
                var result = await _authService.Login(authByLoginDto);

                if (result == null)
                {
                    return BadRequest("Произошла ошибка. Проверьте учетные данные");
                }

                return Ok(result);
            }
            catch
            {
                throw;
            }
        }
    }
}
