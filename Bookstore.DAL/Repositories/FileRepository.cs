using Bookstore.DAL.Interfaces;
using Bookstore.EF;
using File = Bookstore.DAL.Entities.File;

namespace Bookstore.DAL.Repositories;

public class FileRepository(AppDbContext dbContext) : Repository<File>(dbContext), IFileRepository;
