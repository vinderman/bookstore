using Bookstore.BL.Dto.Book;
using Microsoft.AspNetCore.Http;

namespace Bookstore.BL.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetBooks();
        Task<BookDto> GetById(Guid id);
        Task<BookDto> Create(CreateBookDto book);

        // TODO: add format(PDF/EPUB)
        Task<DownloadBookDto> DownloadBook(Guid id);
        //
        Task<bool> UploadBook(IFormFile file, Guid id);
    }
}
