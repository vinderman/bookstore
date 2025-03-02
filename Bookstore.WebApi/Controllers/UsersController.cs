using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Bookstore.BL.Interfaces;
using Bookstore.BL.Dto.User;
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet]
    [Route("currentUser")]
    public async Task<ActionResult<UserDto>> GetUserByToken()
    {
        var token = HttpContext.Request.Headers.Authorization.ToString().Split(' ')[1];
        var user = await _userService.GetUserByJwt(token);

        return Ok(user);
    }
}
