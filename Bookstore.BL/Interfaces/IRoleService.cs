using Bookstore.BL.Dto.Role;

namespace Bookstore.BL.Interfaces;
public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetRoles();
}
