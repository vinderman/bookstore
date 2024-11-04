using Bookstore.BL.Dto.Role;
using Bookstore.BL.Interfaces;
using Bookstore.DAL.Interfaces;

namespace Bookstore.BL.Services;
public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    public async Task<IEnumerable<RoleDto>> GetRoles()
    {
        var roles = await _roleRepository.GetAllAsync();

        return roles.Select(s => new RoleDto { Id = s.Id, Name = s.Name });
    }
}
