using Bookstore.BL.Interfaces;
using Bookstore.BL.Dto;
using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;

namespace Bookstore.BL.Services
{
    public class AuthorService : IAuthorService
    {
        IAuthorRepository _authorRepository;
        IUnitOfWork _unitOfWork;

        public AuthorService(IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
        {
            _authorRepository = authorRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AuthorDto>> GetAuthors(string? search)
        {
            IEnumerable<Author>? authors;

            if (string.IsNullOrWhiteSpace(search))
            {
              authors = await _authorRepository.GetAllAsync();
            }
            else
            {
                authors = await _authorRepository.GetAllBySearch(search);
            }

            return authors == null ? new List<AuthorDto>() : authors.Select(a => new AuthorDto { Id = a.Id, Name = a.Name });
        }

        public async Task<AuthorDto> Create(CreateAuthorDto authorDto)
        {
            var author = new Author
            {
                Name = authorDto.Name,
            };

            await _authorRepository.AddAsync(author);
            await _unitOfWork.SaveChangesAsync();


            return new AuthorDto { Name = author.Name, Id = author.Id };
        }

        public async Task<Guid> SearchAndAddIfNotExists(string search)
        {
            var author = await _authorRepository.FindBySearch(search);

            if (author != null)
            {
                return author.Id;
            }

            var newAuthor = await Create(new CreateAuthorDto { Name = search });
            return newAuthor.Id;
        }
    }
}
