using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;

namespace Bookstore.DAL.EF.Repositories;

public class AuthorRepository : Repository<Author>, IAuthorRepository
{
    public AuthorRepository(AppDbContext dbContext) : base(dbContext) { }

}
