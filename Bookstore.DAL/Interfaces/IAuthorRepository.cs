using Bookstore.DAL.Entities;

namespace Bookstore.DAL.Interfaces;

public interface IAuthorRepository : IRepository<Author>
{
    Task<IEnumerable<Author>> GetAllBySearch(string search);


    Task<Author?> FindBySearch(string search);
}
