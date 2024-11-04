using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.DAL.EF.Repositories;
public class RoleRepository : Repository<Role>, IRoleRepository
{
    AppDbContext _dbContext;

    public RoleRepository(AppDbContext dbContext) : base(dbContext) { }
}
