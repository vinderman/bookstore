﻿using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;
using Bookstore.EF;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.DAL.Repositories;
public class UserRefreshTokenRepository(AppDbContext dbContext) : Repository<UsersRefreshToken>(dbContext), IUserRefreshTokenRepository
{
    private readonly AppDbContext _dbContext = dbContext;
    public async Task<UsersRefreshToken?> FindAsync(string refreshToken)
    {
        var result = await _dbContext.UsersRefreshTokens.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

        return result;
    }
}
