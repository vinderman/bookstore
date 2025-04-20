using Bookstore.BL.Dto.User;

namespace Bookstore.BL.Interfaces;

public interface IUserService
{
    Task<UserDto> GetUserByJwt(string token);
}
