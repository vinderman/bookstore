using Bookstore.DAL.Entities;

namespace Bookstore.DAL.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book?> GetByIsbn(string isbn);
    }
}
