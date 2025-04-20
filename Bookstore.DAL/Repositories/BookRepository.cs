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
    }
}
