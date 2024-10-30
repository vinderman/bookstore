using System.Text.Json.Serialization;

namespace Bookstore.BL.Dto.Auth;
public class AuthByLoginDto
{
    public string Login { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
