using Bookstore.BL.Dto.Auth;
using Bookstore.BL.Interfaces;
using Bookstore.Shared.Exceptions;
using Bookstore.WebApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{

    private readonly IImageProcessorService _imageProcessorService;

    public ImageController(IImageProcessorService imageProcessorService)
    {
        _imageProcessorService = imageProcessorService;
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Файл не предоставлен");
        }

        return Ok(await _imageProcessorService.AddImageToProcess(file));
    }
}
