using System.IdentityModel.Tokens.Jwt;
using Bookstore.BL.Dto.User;
using Bookstore.BL.Enums;
using Bookstore.BL.Interfaces;
using Bookstore.DAL.Interfaces;
using Bookstore.Shared.Exceptions;

namespace Bookstore.BL.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }
    public async Task<UserDto> GetUserByJwt(string token)
    {
        var parsedToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

        var user = await _userRepository.GetByLoginAsync(parsedToken.Subject);

        if (user == null)
        {
            throw new BadRequestException("Пользователь не найден");
        }

        var userRole = await _roleRepository.GetByIdAsync(user.RoleId);

        if (userRole == null)
        {
            throw new BadRequestException("Возникла ошибка при получении роли пользователя");
        }

        Enum.TryParse(userRole.Name, ignoreCase: true, out UserRoleEnum userRoleName);

        return new UserDto
        {
            Id = user.Id,
            Login = user.Login,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            MiddleName = user.MiddleName,
            RoleName = userRoleName,
        };
    }
}
