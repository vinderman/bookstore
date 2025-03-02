namespace Bookstore.BL.Dto.User;

public class UserDto
{
    public Guid Id { get; set; }

    public string Login { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? MiddleName { get; set; }

    public Guid RoleId { get; set; }
}
