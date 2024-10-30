using System;
using System.Collections.Generic;

namespace Bookstore.DAL.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public string Login { get; set; } = null!;

    public string? Email { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Middlename { get; set; }

    public string Password { get; set; } = null!;

    public Guid Roleid { get; set; }

    public virtual Role IdNavigation { get; set; } = null!;
}
