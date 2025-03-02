namespace Bookstore.BL.Dto.Book
{
    public class CreateBookDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid AuthorId { get; set; }

        public IEnumerable<Guid> GenreIds { get; set; }
    }
}
