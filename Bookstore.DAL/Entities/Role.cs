using System;
using System.Collections.Generic;

namespace Bookstore.DAL.Entities;

public partial class Role
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual User? User { get; set; }
}
