using Bookstore.BL.Dto.Genre;

namespace Bookstore.BL.Interfaces;

public interface IGenreService
{
    Task<IEnumerable<GenreDto>> GetGenres();
    Task<bool> Create(CreateGenreDto genre);
}
