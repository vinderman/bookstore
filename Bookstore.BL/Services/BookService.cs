using Bookstore.BL.Interfaces;
using Bookstore.BL.Dto;
using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;

namespace Bookstore.BL
{
    public class BookService: IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository) { 
            _bookRepository = bookRepository;
        }

        public async Task<BookDto> GetById(Guid id)
        {
            var book = await _bookRepository.GetById(id);

            // Добавить маппер
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
            };
        }

        public async Task<BookDto> Create(CreateBookDto request)
        {
            var book = await _bookRepository.Create(new Book
            {
                Description = request.Description,
                Title = request.Title,
            });


            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description, 
            };
        }
    }
}
