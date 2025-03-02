using System;
using System.Collections.Generic;

namespace Bookstore.DAL.Entities;

public class Role
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
