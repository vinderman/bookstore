using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;
using Bookstore.EF;
using Microsoft.EntityFrameworkCore;
using EntityFramework = Microsoft.EntityFrameworkCore;

namespace Bookstore.DAL.Repositories;

public class AuthorRepository(AppDbContext dbContext) : Repository<Author>(dbContext), IAuthorRepository
{
    private readonly AppDbContext _dbContext = dbContext;
    public async Task<IEnumerable<Author>> GetAllBySearch(string search)
    {
        return await _dbContext.Authors.Where(a => EntityFramework.EF.Functions.Like(a.Name.ToLower(), $"%{search.ToLower()}%")).ToListAsync();
    }

    public async Task<Author?> FindBySearch(string search)
    {
        return await _dbContext.Authors.Where(a => EntityFramework.EF.Functions.Like(a.Name.ToLower(), $"%{search.ToLower()}%")).FirstOrDefaultAsync();
    }
}
