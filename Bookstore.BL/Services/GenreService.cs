using Bookstore.BL.Dto.Genre;
using Bookstore.BL.Interfaces;
using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;

namespace Bookstore.BL.Services;

public class GenreService(IGenreRepository genreRepository, IUnitOfWork unitOfWork) : IGenreService
{
    public async Task<IEnumerable<GenreDto>> GetGenres()
    {
        var genres = await genreRepository.GetAllAsync();

        if (genres == null)
        {
            return new List<GenreDto>();
        }

        return genres.Select(g => new GenreDto { Id = g.Id, Name = g.Name });
    }
    public async Task<bool> Create(CreateGenreDto genreDto)
    {
        var payload = new Genre{ Name = genreDto.Title };
        await unitOfWork.BeginTransactionAsync();
        await genreRepository.AddAsync(payload);
        await unitOfWork.CommitTransactionAsync();

        return true;
    }
}
