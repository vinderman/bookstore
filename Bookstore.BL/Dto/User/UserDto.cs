using Bookstore.BL.Enums;

namespace Bookstore.BL.Dto.User;

public class UserDto
{
    public Guid Id { get; set; }

    public required string Login { get; set; }

    public string? Email { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? MiddleName { get; set; }

    public required UserRoleEnum RoleName { get; set; }
}
