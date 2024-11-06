using System;
using System.Collections.Generic;

namespace Bookstore.DAL.Entities;

public partial class Book
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid AuthorId { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
