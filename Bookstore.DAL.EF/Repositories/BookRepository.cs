using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.DAL.EF.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
