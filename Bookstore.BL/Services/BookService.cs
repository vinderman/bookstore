using Bookstore.BL.Interfaces;
using Bookstore.BL.Dto;
using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;

namespace Bookstore.BL.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;
    public BookService(IBookRepository bookRepository, IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;

    }

    public async Task<BookDto> GetById(Guid id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
        {
            throw new KeyNotFoundException("Книги с данным идентификатором не найдено");
        }

        // Добавить маппер
        return new BookDto
        {
            Id = book.Id,
            Title = book.Name,
            Description = book.Description,
        };
    }

    public async Task<BookDto> Create(CreateBookDto request)
    {
        var book = new Book
        {
            Description = request.Description,
            Name = request.Name,
        };

        await _unitOfWork.BeginTransactionAsync();
        await _bookRepository.AddAsync(book);
        await _unitOfWork.CommitTransactionAsync();


        return new BookDto
        {
            Id = book.Id,
            Title = book.Name,
            Description = book.Description,
        };
    }
}
