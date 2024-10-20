using Bookstore.BL.Dto;

namespace Bookstore.BL.Interfaces
{
    public interface IAuthorService
    {
        //Task<BookDto> GetById(Guid id);
        Task<AuthorDto> Create(CreateAuthorDto book);
    }
}