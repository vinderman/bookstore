using File = Bookstore.DAL.Entities.File;

namespace Bookstore.DAL.Interfaces;

public interface IFileRepository: IRepository<File>
{
        Task<IEnumerable<File>> GetByBookIdAsync(Guid bookId);

        Task<IEnumerable<File>> GetByCorrespondingBookIds(IEnumerable<Guid> correspondingBookIds);
}
