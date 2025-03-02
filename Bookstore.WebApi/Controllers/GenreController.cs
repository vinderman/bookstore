using Bookstore.BL.Dto.Genre;
using Bookstore.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenreController: ControllerBase
{
    private readonly IGenreService _genreService;
    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<IEnumerable<GenreDto>> GetGenres()
    {
        return await _genreService.GetGenres();
    }

    [HttpPost]
    public async Task<bool> Create(CreateGenreDto genreDto)
    {
        return await _genreService.Create(genreDto);
    }

}
