using System;
using System.Collections.Generic;

namespace Bookstore.DAL.Entities;

public partial class UsersRefreshToken
{
    public string RefreshToken { get; set; } = null!;
}
