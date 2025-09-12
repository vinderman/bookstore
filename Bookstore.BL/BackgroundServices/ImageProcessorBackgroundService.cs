using Bookstore.BL.Interfaces;
using Bookstore.BL.Services;
using Bookstore.BL.Services.ImageProcessing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bookstore.BL.BackgroundServices;

public class ImageProcessingBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ImageProcessingBackgroundService> _logger;

    public ImageProcessingBackgroundService(IServiceProvider serviceProvider, ILogger<ImageProcessingBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var imageProcessor = scope.ServiceProvider.GetRequiredService<IImageProcessorService>();

                    var image = await imageProcessor.GetFirstNotProcessedImage();

                    if (image != null)
                    {

                        var instance = new PaintByNumberGenerator();

                        instance.ProcessImage(image.OriginalImagePath, 25, Path.Combine($"../processed/{image.Id}-1.png"),
                            Path.Combine($"../processed/{image.Id}-2.png"));

                        image.Status = "Processed";
                        await imageProcessor.UpdateStatus(image);
                        Console.WriteLine("PROCESSED");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в фоновом сервисе");
            }

            // Ждем перед следующей проверкой
            await Task.Delay(5000, stoppingToken); // Проверяем каждые 5 секунд
        }
    }
}


