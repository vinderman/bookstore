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
			var book = await _context.Book.FirstOrDefaultAsync(book => book.id == id);

			return book;
		}

		public async Task<Book> Create(Book book)
		{
			var result = await _context.Book.AddAsync(book);
			await _context.SaveChangesAsync();

			var createdBook = await _context.Book.FirstOrDefaultAsync(b => b.title == book.title);

			return createdBook;
		}
	}
}