using System.ComponentModel;

namespace Bookstore.BL.Enums;

public enum UserRoleEnum
{
    [Description("Администратор")]
    Admin,
    [Description("Пользователь")]
    User
}
