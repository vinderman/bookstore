using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;
using Bookstore.EF;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.DAL.Repositories;

public class UserRepository(AppDbContext dbContext) : Repository<User>(dbContext), IUserRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<User?> GetByLoginAsync(string login)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == login);
    }


}
