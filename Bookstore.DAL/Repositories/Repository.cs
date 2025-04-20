using System.Linq.Expressions;
using Bookstore.EF;
using Bookstore.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.DAL.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }

    public void UpdateProperties(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
    {
        _context.Attach(entity);

        foreach (var property in properties)
        {
            _context.Entry(entity).Property(property).IsModified = true;
        }
    }

    public async Task Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }
}
