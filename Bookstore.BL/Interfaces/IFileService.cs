using Bookstore.BL.Dto.File;
using Microsoft.AspNetCore.Http;

namespace Bookstore.BL.Interfaces;

public interface IFileService
{
    // TODO: add format(PDF/EPUB)
    Task<DownloadFileDto> DownloadFile(Guid id);
    //
    Task<bool> UploadFile(IFormFile file, Guid id);
}
