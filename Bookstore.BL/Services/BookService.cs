using System.Collections;
using System.Transactions;
using Bookstore.BL.Dto;
using Bookstore.BL.Interfaces;
using Bookstore.BL.Dto.Book;
using Bookstore.BL.Dto.File;
using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;
using Bookstore.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using File = Bookstore.DAL.Entities.File;

namespace Bookstore.BL.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenreRepository _genreRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IFileService _fileService;
    private readonly IFileRepository _fileRepository;
    private readonly IAuthorService _authorService;
    public BookService(IBookRepository bookRepository, IUnitOfWork unitOfWork, IFileService fileService, IGenreRepository genreRepository, IAuthorRepository authorRepository, IAuthorService authorService, IFileRepository fileRepository)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
        _genreRepository = genreRepository;
        _authorRepository = authorRepository;
        _fileService = fileService;
        _authorService = authorService;
        _fileRepository = fileRepository;
    }

    public async Task<IEnumerable<BookDto>> GetBooks()
    {
        var books = await _bookRepository.GetAllAsync();

        if (books == null)
        {
            return new List<BookDto>();
        }

        var authorsIds = new List<Guid>();
        var bookIds = new List<Guid>();
        foreach (var book in books)
        {
            authorsIds.Add(book.AuthorId);
            bookIds.Add(book.Id);

        }
        var authors = await _authorRepository.GetAllByIds(authorsIds);
        var files = await _fileRepository.GetByCorrespondingBookIds(bookIds);


        return books.Select(b =>
        {
            var correspondingAuthor = authors.FirstOrDefault(a => a.Id == b.AuthorId);
            if (correspondingAuthor == null)
            {
                throw new BadRequestException($"Не найден автор для книги {b.Name}");
            }

            var correspondingFile = files.FirstOrDefault(f => f.BookId == b.Id);
            var filesDto = new List<FileDto>();
            if (correspondingFile != null)
            {
                filesDto.Add(new FileDto
                {
                    Id = correspondingFile.Id,
                    Name = correspondingFile.Name,
                    Size = correspondingFile.FileSize,
                    Extension = correspondingFile.FileType
                });
            }


            {
                return new BookDto
                {
                    Id = b.Id,
                    Title = b.Name,
                    Description = b.Description,
                    Author = new AuthorDto { Name = correspondingAuthor.Name, Id = correspondingAuthor.Id },
                    Files = filesDto
                };
            }
        });
    }

    public async Task<BookDto> GetById(Guid id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
        {
            throw new KeyNotFoundException("Книги с данным идентификатором не найдено");
        }

        var author = await _authorRepository.GetByIdAsync(book.AuthorId);

        if (author == null)
        {
            throw new BadRequestException($"Не найден автор для книги {book.Name}");
        }

        var files = await _fileRepository.GetByBookIdAsync(book.Id);


        return new BookDto
        {
            Id = book.Id,
            Title = book.Name,
            Description = book.Description,
            Author = new AuthorDto { Id = author.Id, Name = author.Name },
            Files = files.Select(f => new FileDto { Id = f.Id, Name = f.Name, Size = f.FileSize, Extension = f.FileType })
        };
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
        var author = await _authorRepository.GetByIdAsync(book.AuthorId);
        if (author == null)
        {
            throw new BadRequestException($"Не найден автор для книги {book.Name}");
        }
        await _unitOfWork.CommitTransactionAsync();


        return new BookDto
        {
            Id = book.Id, Title = book.Name, Description = book.Description, Author = new AuthorDto { Id = author.Id, Name = author.Name }
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
                Console.WriteLine($"BOOK UPLOADED {bookToUpload.Name}");
            }

            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.WriteLine("Ошибка преобразования книги");
            await _unitOfWork.RollbackTransactionAsync();
        }
    }
}
