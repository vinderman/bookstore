using System.Text.Json.Serialization;

namespace Bookstore.BL.Dto.Role;
public class RoleDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
}
