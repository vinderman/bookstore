using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;
using Bookstore.EF;

namespace Bookstore.DAL.Repositories;

public class GenreRepository(AppDbContext dbContext) : Repository<Genre>(dbContext), IGenreRepository;
