using System.Collections;
using System.Transactions;
using Bookstore.BL.Interfaces;
using Bookstore.BL.Dto.Book;
using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;
using Bookstore.Shared.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Bookstore.BL.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenreRepository _genreRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IFileService _fileService;
    private readonly IAuthorService _authorService;
    public BookService(IBookRepository bookRepository, IUnitOfWork unitOfWork, IFileService fileService, IGenreRepository genreRepository, IAuthorRepository authorRepository, IAuthorService authorService)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
        _genreRepository = genreRepository;
        _authorRepository = authorRepository;
        _fileService = fileService;
        _authorService = authorService;
    }

    public async Task<IEnumerable<BookDto>> GetBooks()
    {
        var books = await _bookRepository.GetAllAsync();

        if (books == null)
        {
            return new List<BookDto>();
        }

        // TODO: прокидывать информацию о связанных файлах
        return books.Select(b =>
            new BookDto { Id = b.Id, Title = b.Name, Description = b.Description, AuthorId = b.AuthorId});
    }

    public async Task<BookDto> GetById(Guid id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
        {
            throw new KeyNotFoundException("Книги с данным идентификатором не найдено");
        }



        // TODO: Добавить маппер, прокинуть Genres
        // TODO: прокинуть связанные файлы
        return new BookDto { Id = book.Id, Title = book.Name, Description = book.Description, AuthorId = book.AuthorId };
    }

    public async Task<BookDto> Create(CreateBookDto request)
    {
        var book = new Book
        {
            Description = request.Description,
            Name = request.Name,
            AuthorId = request.AuthorId,
            Genres = new List<Genre>()
        };

        // Добавляем связи с указанными жанрами
        foreach (var genreId in request.GenreIds)
        {
            var genre = await _genreRepository.GetByIdAsync(genreId);
            if (genre != null)
            {
                book.Genres.Add(genre);
            }
        }

        await _unitOfWork.BeginTransactionAsync();
        await _bookRepository.AddAsync(book);
        await _unitOfWork.CommitTransactionAsync();


        return new BookDto
        {
            Id = book.Id, Title = book.Name, Description = book.Description, AuthorId = book.AuthorId,
        };
    }

    /// <summary>
    /// Удаление книги
    /// </summary>
    /// <param name="id"></param>
    public async Task Delete(Guid id)
    {
        // TODO: добавить также удаление файла
        await _bookRepository.Delete(new Book { Id = id });
    }

    public async Task ProcessBookImport(BookMetadata bookMetadata, Stream file)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var existingBook = await _bookRepository.GetByIsbn(bookMetadata.Isbn);
            if (existingBook == null)
            {
                var authorId = await _authorService.SearchAndAddIfNotExists(bookMetadata.Author);


                var bookToUpload = new Book { Isbn = bookMetadata.Isbn, Name = bookMetadata.Title, Description = bookMetadata.Description };
                bookToUpload.AuthorId = authorId;
                await _bookRepository.AddAsync(bookToUpload);
                await _unitOfWork.SaveChangesAsync();

                await _fileService.UploadFile(new FormFile(file, 0, file.Length, bookMetadata.Title, bookMetadata.Title), bookToUpload.Id);

                await _unitOfWork.CommitTransactionAsync();
                Console.WriteLine($"BOOK UPLOADED {bookToUpload.Name}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.WriteLine("Ошибка преобразования книги");
            await _unitOfWork.RollbackTransactionAsync();
        }
    }
}
