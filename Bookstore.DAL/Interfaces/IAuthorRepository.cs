using Bookstore.DAL.Entities;

namespace Bookstore.DAL.Interfaces;

public interface IAuthorRepository : IRepository<Author>
{
    Task<IEnumerable<Author>?> GetBySearch(string search);
}
