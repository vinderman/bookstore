using Bookstore.BL;
using Bookstore.BL.Interfaces;
using Bookstore.BL.Services;
using Bookstore.DAL.EF;
using Bookstore.DAL.EF.Repositories;
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
            options.UseNpgsql(configuration.GetConnectionString("Default")));

            InjectRepositories(services);
            InjectServices(services);
        }

        public static void InjectRepositories(IServiceCollection services)
        {
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
        }

        public static void InjectServices(IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthorService, AuthorService>();
        }
    }
}


