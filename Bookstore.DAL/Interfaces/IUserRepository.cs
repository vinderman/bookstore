using Bookstore.DAL.Entities;

namespace Bookstore.DAL.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByLoginAsync(string login);
}
