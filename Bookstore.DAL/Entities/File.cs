namespace Bookstore.DAL.Entities;

public class File
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string FileHash { get; set; } = string.Empty;

    public int FileSize { get; set; } = 0;

    public string FileType { get; set; } = string.Empty;

    public string S3Url { get; set; } = string.Empty;

    public DateTime UploadedAt { get; set; }

    public Guid BookId { get; set; }
    public Book Book { get; set; }
}
