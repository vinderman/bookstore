using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.DAL.EF.Repositories
{
	public class BookRepository: IBookRepository
	{
		AppDbContext _dbContext;

		public BookRepository(AppDbContext dbContext) {
            _dbContext = dbContext;
		}

		public async Task<Book> GetById(Guid id)
		{
			var book = await _dbContext.Books.FirstOrDefaultAsync(book => book.Id == id);

			return book;
		}

		public async Task<Book> Create(Book book)
		{
			await _dbContext.Books.AddAsync(book);
			await _dbContext.SaveChangesAsync();

			return book;
		}
	}
}