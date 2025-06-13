using System.Data;

namespace Bookstore.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted);
        Task CommitTransactionAsync();

        Task SaveChangesAsync();
        Task RollbackTransactionAsync();
    }
}
