using Bookstore.BL.Dto.Auth;

namespace Bookstore.BL.Interfaces;
public interface IAuthService
{
    Task<AuthByLoginResponseDto> Login(AuthByLoginDto loginDto);

    Task<bool> Register(RegisterDto registerDto);
}
