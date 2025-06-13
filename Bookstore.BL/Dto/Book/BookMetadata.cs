using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bookstore.BL.Dto.Book;

public class BookMetadata
{
    [JsonPropertyName("author")]
    public string Author { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("year")]
    public int? Year{ get; set; }

    [JsonPropertyName("language")] public string? Language { get; set; }
    [JsonPropertyName("isbn")]
    public required string Isbn { get; set; }
    [JsonPropertyName("filePath")]
    public required string FilePath { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}
