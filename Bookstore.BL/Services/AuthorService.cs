using Bookstore.BL.Interfaces;
using Bookstore.BL.Dto;
using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;

namespace Bookstore.BL.Services
{
    public class AuthorService: IAuthorService
    {
        IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<AuthorDto> Create(CreateAuthorDto authorDto)
        {
            var result = await _authorRepository.Create(new Author
            {
                Name = authorDto.Name,
            });

            if (result == null)
            {
                throw new Exception("Не удалось создать автора");
            }


            return new AuthorDto { Name = result.Name, Id = result.Id };
        }
    }
}
