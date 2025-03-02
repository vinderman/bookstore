using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;
using Bookstore.EF;

namespace Bookstore.DAL.Repositories;

public class AuthorRepository(AppDbContext dbContext) : Repository<Author>(dbContext), IAuthorRepository;
