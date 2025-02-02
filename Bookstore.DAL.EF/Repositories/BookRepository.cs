using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.DAL.EF.Repositories
{
    public class BookRepository(AppDbContext dbContext) : Repository<Book>(dbContext), IBookRepository
    {
    }
}
