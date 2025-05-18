using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Bookstore.BL.Dto.File;

public class UploadFileDto
{
    [Required]
    public IFormFile file { get; set; }

    [Required]
    public string fileName { get; set; }
}
