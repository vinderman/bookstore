using Bookstore.DAL.Entities;

namespace Bookstore.DAL.Interfaces;
public interface IUserRefreshTokenRepository : IRepository<UsersRefreshToken>
{
    Task<UsersRefreshToken> FindAsync(string refreshToken);
}
