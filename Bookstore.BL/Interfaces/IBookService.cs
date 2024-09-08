using Bookstore.BL.Dto;

namespace Bookstore.BL.Interfaces
{
    public interface IBookService
    {
        Task<BookDto>  GetById(Guid id);
        Task<BookDto> Create(CreateBookDto book);
    }
}