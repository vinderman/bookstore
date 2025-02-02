using System.IdentityModel.Tokens.Jwt;
using Bookstore.BL.Dto.Role;
using Bookstore.BL.Dto.User;
using Bookstore.BL.Interfaces;
using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;
using Bookstore.Shared.Exceptions;

namespace Bookstore.BL.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<UserDto> GetUserByJwt(string token)
    {
        var parsedToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

        var user = await _userRepository.GetByLoginAsync(parsedToken.Subject);

        if (user == null)
        {
            throw new BadRequestException("Пользователь не найден");
        }

        return new UserDto
        {
            Id = user.Id,
            Login = user.Login,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            MiddleName = user.MiddleName,
            RoleId = user.RoleId,
        };
    }
}
