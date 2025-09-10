using Bookstore.BL.BackgroundServices;
using Bookstore.BL.Interfaces;
using Bookstore.BL.Mapper;
using Bookstore.BL.Services;
using Bookstore.EF;
using Bookstore.DAL.Repositories;
using Bookstore.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookstore.DI
{
    public static class DependencyInjector
    {
        public static void InjectDependencies(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default"),
                x => x.MigrationsAssembly("Bookstore.WebApi"))
            );

            services.AddAutoMapper(config => config.AddProfile<MapperProfile>());

            InjectRepositories(services);
            InjectServices(services);
            services.AddHostedService<RefreshTokenBackgroundService>();
            services.AddHostedService<BookImporterBackgroundService>();
        }

        private static void InjectRepositories(IServiceCollection services)
        {
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IUserRefreshTokenRepository, UserRefreshTokenRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
        }

        private static void InjectServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IFileService, FileService>();
        }
    }
}


