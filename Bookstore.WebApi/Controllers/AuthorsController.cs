using Microsoft.AspNetCore.Mvc;
using Bookstore.BL.Dto;
using Bookstore.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookstore.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<AuthorDto>> Create(CreateAuthorDto request)
        {

            var createdBook = await _authorService.Create(request);

            return Ok(createdBook);
        }
    }
}
