using Bookstore.DAL.Interfaces;
using Bookstore.EF;
using Microsoft.EntityFrameworkCore;
using File = Bookstore.DAL.Entities.File;

namespace Bookstore.DAL.Repositories;

public class FileRepository(AppDbContext dbContext) : Repository<File>(dbContext), IFileRepository
{
    public async Task<IEnumerable<File>> GetByBookIdAsync(Guid bookId)
    {
        return await dbContext.Files.Where(f => f.BookId == bookId).ToListAsync();
    }

    public async Task<IEnumerable<File>> GetByCorrespondingBookIds(IEnumerable<Guid> correspondingBookIds)
    {
        return await dbContext.Files.Where(f => correspondingBookIds.Contains(f.BookId)).ToListAsync();
    }
}
