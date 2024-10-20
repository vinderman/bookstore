namespace Bookstore.BL.Dto
{
    using System;

    public class BookDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;

        public Guid AuthorId { get; set; }
    }

}
