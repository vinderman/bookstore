using Bookstore.DAL.Entities;
using Microsoft.AspNetCore.Http;

namespace Bookstore.BL.Interfaces;

public interface IImageProcessorService
{
    public Task<Guid> AddImageToProcess(IFormFile file);

    public Task<ImageProcessingTask> GetFirstNotProcessedImage();

    public Task<ImageProcessingTask> UpdateStatus(ImageProcessingTask image);
}
