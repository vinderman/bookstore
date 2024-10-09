namespace Bookstore.BL.Dto
{
    using System;

    public class BookDto
    {
        public Guid id { get; set; }

        public string title { get; set; } = String.Empty;

        public string description { get; set; } = String.Empty;

        public Guid authorId { get; set; }
    }

}
