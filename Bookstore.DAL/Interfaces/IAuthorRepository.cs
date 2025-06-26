using Bookstore.DAL.Entities;

namespace Bookstore.DAL.Interfaces;

public interface IAuthorRepository : IRepository<Author>
{
    Task<IEnumerable<Author>> GetAllBySearch(string search);

    Task<IEnumerable<Author>> GetAllByIds(IEnumerable<Guid> authorIds);


    Task<Author?> FindBySearch(string search);
}
