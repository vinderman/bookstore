using Bookstore.BL.Dto.Book;
using Microsoft.AspNetCore.Http;

namespace Bookstore.BL.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetBooks();
        Task<BookDto> GetById(Guid id);
        Task<BookDto> Create(CreateBookDto book);

        Task Delete(Guid id);

        Task ProcessBookImport(BookMetadata bookMetadata, Stream file);
    }
}
