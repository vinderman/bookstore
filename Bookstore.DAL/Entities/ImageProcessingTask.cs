namespace Bookstore.DAL.Entities;

public class ImageProcessingTask
{
    public Guid Id { get; set; }
    public string OriginalImagePath { get; set; } // Путь к исходному файлу
    public string? ProcessedImagePath { get; set; } // Путь к обработанной схеме
    public string Status { get; set; } // "Uploaded", "Processing", "Completed", "Failed"
    public DateTime CreatedAt { get; set; }
    public string? ErrorMessage { get; set; }
}
