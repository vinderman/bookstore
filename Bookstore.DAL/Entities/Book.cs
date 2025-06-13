using System.ComponentModel.DataAnnotations;

namespace Bookstore.DAL.Entities;

public class Book
{
    public Guid Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(300)]
    public string Description { get; set; }

    public Guid AuthorId { get; set; }

    public string Isbn { get; set; }

    public Author Author { get; set; }
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public ICollection<File> Files { get; set; } = new List<File>();
}
