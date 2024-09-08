using Bookstore.BL;
using Bookstore.BL.Interfaces;
using Bookstore.DAL.EF;
using Bookstore.DAL.EF.Repositories;
using Bookstore.DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Bookstore.DI
{
    public static class DependencyInjector
    {
        public static void InjectDependencies(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IBookService, BookService>();
        }
    }
}


