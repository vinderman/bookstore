
namespace Bookstore.DAL.Entities;

public class Genre
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public ICollection<Book> Books { get; } = new List<Book>();
}
