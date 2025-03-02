namespace Bookstore.BL.Dto.Book
{
    using System;

    public class BookDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

       public FileDto? file { get; set; }

        public Guid AuthorId { get; set; }
    }

}
