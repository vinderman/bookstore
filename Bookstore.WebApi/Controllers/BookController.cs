using Microsoft.AspNetCore.Mvc;
using Bookstore.WebApi.Models;
using Bookstore.DAL.EF;
using Bookstore.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController: ControllerBase
    {
        private readonly AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
    	public ActionResult<BookDto> Get()
    	{
            var result = _context.Book.First();
            return Ok(result);
            
    	}

        [HttpPost]
        public async Task<ActionResult<BookDto>> Create(BookDto request)
        {

            var book = await _context.Book.AddAsync(new Book
            {
                title = request.title,
                description = request.description,
            });

            await _context.SaveChangesAsync();
            var createdBook = await _context.Book.FirstOrDefaultAsync(b => b.title == request.title);

            return Ok(createdBook);
        }

    }
}
