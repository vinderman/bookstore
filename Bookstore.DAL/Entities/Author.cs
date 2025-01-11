using System;
using System.Collections.Generic;

namespace Bookstore.DAL.Entities;

public partial class Author
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
