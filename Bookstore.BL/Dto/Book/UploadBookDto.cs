using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Bookstore.BL.Dto.Book;

public class UploadBookDto
{
    [Required]
    public IFormFile file { get; set; }

    [Required]
    public string fileName { get; set; }
}
