using Bookstore.DAL.Entities;

namespace Bookstore.DAL.Interfaces
{
    public interface IAuthorRepository
    {
        Task<Author> Create(Author author);
    }
}
