using System.Text.Json;
using System.Text.RegularExpressions;
using Bookstore.BL.Dto.Book;
using Bookstore.BL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace Bookstore.BL.BackgroundServices;

public class BookImporterBackgroundService(IServiceProvider serviceProvider): BackgroundService
{
    private readonly string _pathToBooksMeta = "/Users/evgeniykalmykov/Documents/Книги/books_meta.json";
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await DoWork(stoppingToken);
        }
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var bookService =
                scope.ServiceProvider
                    .GetRequiredService<IBookService>();

            // var fileService = scope.ServiceProvider.GetRequiredService<IFileService>();

            var fileString = File.ReadAllText(_pathToBooksMeta);

            try
            {
                var books = JsonSerializer.Deserialize<List<BookMetadata>>(fileString);

                foreach (var book in books)
                {
                    var file = File.OpenRead(book.FilePath);


                    await bookService.ProcessBookImport(book, file);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Console.WriteLine("Ошибка парсинга файла");
            }
        }

        await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        await base.StopAsync(stoppingToken);
    }


}
