using Bookstore.BL.Dto;

namespace Bookstore.BL.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDto>> GetAuthors(string? search);
        Task<AuthorDto> Create(CreateAuthorDto book);
        Task<Guid> SearchAndAddIfNotExists(string search);
    }
}
