using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;
using Bookstore.EF;

namespace Bookstore.DAL.Repositories;
public class RoleRepository(AppDbContext dbContext) : Repository<Role>(dbContext), IRoleRepository;
