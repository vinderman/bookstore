using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.DAL.EF.Repositories
{
    public class AuthorRepository: IAuthorRepository
    {
        AppDbContext _dbContext;

        public AuthorRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Author> Create(Author author)
        {
            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();

            return author;
        }
    }
}
