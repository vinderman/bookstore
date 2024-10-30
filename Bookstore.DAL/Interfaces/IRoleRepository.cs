using Bookstore.DAL.Entities;
namespace Bookstore.DAL.Interfaces;
public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAll();
}
