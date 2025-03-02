using Bookstore.BL.Dto.User;
using Bookstore.DAL.Entities;

namespace Bookstore.BL.Interfaces;

public interface IUserService
{
    Task<UserDto> GetUserByJwt(string token);
}
