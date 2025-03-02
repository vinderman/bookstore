

using System.Linq.Expressions;

namespace Bookstore.DAL.Interfaces;
public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>?> GetAllAsync();
    Task<TEntity?> GetByIdAsync(Guid id);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);

    void UpdateProperties(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
    void Delete(TEntity entity);
}
