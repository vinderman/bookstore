using Bookstore.BL.Dto.File;

namespace Bookstore.BL.Dto.Book
{
    using System;

    public class BookDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public AuthorDto Author { get; set; }

        public IEnumerable<FileDto> Files { get; set; }
    }

}
