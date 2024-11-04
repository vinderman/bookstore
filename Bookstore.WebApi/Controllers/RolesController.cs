using Microsoft.AspNetCore.Mvc;
using Bookstore.BL.Interfaces;
using Bookstore.BL.Dto;
using Bookstore.BL.Dto.Role;

namespace Bookstore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetAll()
    {
        try
        {
            var roles = await _roleService.GetRoles();

            if (roles == null)
            {
                return BadRequest("Произошла ошибка. Проверьте учетные данные");
            }

            return Ok(roles);
        }
        catch
        {
            throw;
        }



    }
}
