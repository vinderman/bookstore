using System;
using Microsoft.EntityFrameworkCore;
using Bookstore.DAL.Entities;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Bookstore.DAL.EF.Mappers;

namespace Bookstore.DAL.EF
{
    /// <summary>
    /// Базовый класс для работы с БД
    /// </summary>
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
            Database.EnsureCreated();
            Console.WriteLine("Database created");
        }

        // Добавить получение ConnectionString из appsetting(далее - из env)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=bookstore;Username=backend;Password=12345;");
        }

        public DbSet<Book> Book { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookMap());
        }
    }
}
