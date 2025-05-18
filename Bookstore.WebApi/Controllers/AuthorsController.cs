using Microsoft.AspNetCore.Mvc;
using Bookstore.BL.Dto;
using Bookstore.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookstore.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors([FromQuery(Name = "search")] string? search)
    {
        return Ok(await _authorService.GetAuthors(search));
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<AuthorDto>> Create(CreateAuthorDto request)
    {

        var createdBook = await _authorService.Create(request);

        return Ok(createdBook);
    }
}
