using System.Collections;
using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;
using Bookstore.EF;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.DAL.Repositories
{
    public class BookRepository(AppDbContext dbContext) : Repository<Book>(dbContext), IBookRepository
    {
        public async Task Create(Book book)
        {
            dbContext.Books.Add(book);
        }

        public async Task<Book?> GetByIsbn(string isbn)
        {
            return await dbContext.Books.Where(b => b.Isbn == isbn).FirstOrDefaultAsync();
        }
    }
}
