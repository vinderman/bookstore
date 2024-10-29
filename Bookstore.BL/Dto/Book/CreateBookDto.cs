namespace Bookstore.BL.Dto
{
    public class CreateBookDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid AuthorId { get; set; }
    }
}
