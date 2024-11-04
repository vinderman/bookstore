using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.DAL.EF.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly AppDbContext _dbContext;
    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByLoginAsync(string login)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == login);
    }


}
