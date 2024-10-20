using System;
using System.Collections.Generic;

namespace Bookstore.DAL.Entities;

public partial class Book
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public Guid Authorid { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
