﻿using Bookstore.BL.Interfaces;
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
    private readonly IDocumentRepository _documentRepository;
    private readonly IGenreRepository _genreRepository;
    public BookService(IBookRepository bookRepository, IUnitOfWork unitOfWork, IDocumentRepository documentRepository, IGenreRepository genreRepository)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
        _documentRepository = documentRepository;
        _genreRepository = genreRepository;


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

    public async Task Delete(Guid id)
    {
        _bookRepository.Delete(new Book { Id = id });
    }
}
