using Bookstore.DAL.Entities;

namespace Bookstore.DAL.Interfaces
{
	public interface IBookRepository
	{
		Task<Book> GetById(Guid id);

		Task<Book> Create(Book book);
	}
}