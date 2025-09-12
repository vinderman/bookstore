using Bookstore.BL.Interfaces;
using Bookstore.DAL.Entities;
using Bookstore.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.BL.Services
{
    public class ImageProcessorService : IImageProcessorService
    {

        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;



        public ImageProcessorService(AppDbContext context, IWebHostEnvironment environment)
        {
            _dbContext = context;
            _environment = environment;
        }

        public async Task<Guid> AddImageToProcess(IFormFile file)
        {
            var fileName = $"{file.FileName}";
            var filePath = Path.Combine("../", "uploads", fileName);



            // Создаем папку, если её нет
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            // Сохраняем файл
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }


            var task = new ImageProcessingTask
            {
                OriginalImagePath = filePath,
                Status = "Uploaded",
                CreatedAt = DateTime.UtcNow
            };

            var result=  _dbContext.ImageProcessingTask.Add(task);
           await _dbContext.SaveChangesAsync();

           return result.Entity.Id;
        }

        public async Task<ImageProcessingTask?> GetFirstNotProcessedImage()
        {
            return await _dbContext.ImageProcessingTask.Where(p => p.Status == "Uploaded").FirstOrDefaultAsync();
        }

        public async Task<ImageProcessingTask> UpdateStatus(ImageProcessingTask imageProcessingTask)
        {
            _dbContext.Update(imageProcessingTask);
            await _dbContext.SaveChangesAsync();

            return imageProcessingTask;
        }
    }
}
