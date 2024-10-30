using System;
using System.Collections.Generic;

namespace Bookstore.DAL.Entities;

public partial class Genre
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
