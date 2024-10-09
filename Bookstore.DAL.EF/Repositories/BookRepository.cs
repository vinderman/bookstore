using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.DAL.EF.Repositories
{
	public class BookRepository: IBookRepository
	{
		AppDbContext _context;

		public BookRepository(AppDbContext context) {
			_context = context;
		}

		public async Task<Book> GetById(Guid id)
		{
			var book = await _context.Books.FirstOrDefaultAsync(book => book.Id == id);

			return book;
		}

		public async Task<Book> Create(Book book)
		{
			var result = await _context.Books.AddAsync(book);
			await _context.SaveChangesAsync();

			var createdBook = await _context.Books.FirstOrDefaultAsync(b => b.Title == book.Title);

			return createdBook;
		}
	}
}