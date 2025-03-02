﻿using System.ComponentModel.DataAnnotations;

namespace Bookstore.DAL.Entities;

public class Book
{
    public Guid Id { get; init; }

    [MaxLength(100)]
    public string Name { get; init; }

    [MaxLength(300)]
    public string Description { get; init; }

    public Guid AuthorId { get; init; }

    [MaxLength(100)]
    public string? FileId { get; init; }

    [MaxLength(100)]
    public string? FileName { get; init; }

    public Author Author { get; init; }
    public ICollection<Genre> Genres { get; init; } = new List<Genre>();
}
