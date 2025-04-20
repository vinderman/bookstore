using Microsoft.AspNetCore.Mvc;
using Bookstore.BL.Interfaces;
using Bookstore.BL.Dto.Book;

namespace Bookstore.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<IEnumerable<BookDto>> Get()
    {
        var result = await _bookService.GetBooks();

        return result;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> Get([FromRoute] Guid id)
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

    [HttpGet("{id}/file")]
    public async Task<ActionResult<bool>> DownloadFile([FromRoute] Guid id)
    {
        var downloadBookDto = await _bookService.DownloadBook(id);

        return File(downloadBookDto.FileContent, "application/pdf", downloadBookDto.Name);
    }

    [HttpPost("{id}/file")]
    public async Task<ActionResult<bool>> UploadFile([FromRoute] Guid id, [FromForm] UploadBookDto request)
    {
        var isUploaded = await _bookService.UploadBook(request.file, id);

        return isUploaded;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _bookService.Delete(id);

        return NoContent();
    }
}
