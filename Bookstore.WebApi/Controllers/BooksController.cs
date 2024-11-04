using Microsoft.AspNetCore.Mvc;
using Bookstore.BL.Interfaces;
using Bookstore.BL.Dto;

namespace Bookstore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> Get(Guid id)
    {
        var result = await _bookService.GetById(id);
        return Ok(result);

    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> Create(CreateBookDto request)
    {

        var createdBook = await _bookService.Create(request);

        return Ok(createdBook);
    }

}
