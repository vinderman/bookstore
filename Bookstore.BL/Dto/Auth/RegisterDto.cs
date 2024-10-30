namespace Bookstore.BL.Dto.Auth;
public class RegisterDto
{
    public string Login { get; set; } = String.Empty;

    public string? Email { get; set; } = String.Empty;

    public string Firstname { get; set; } = String.Empty;

    public string Lastname { get; set; } = String.Empty;

    public string? Middlename { get; set; } = String.Empty;

    public string Password { get; set; } = String.Empty;
}
