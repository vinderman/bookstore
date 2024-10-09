namespace Bookstore.BL.Dto
{
    public class CreateBookDto
    {
        public string title { get; set; }

        public string description { get; set; }

        public Guid authorId { get; set; }
    }
}