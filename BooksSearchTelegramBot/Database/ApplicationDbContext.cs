using BooksSearchTelegramBot.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksSearchTelegramBot.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserReadedBook> UserReadedBook { get; set; }
        public DbSet<UserDeferredBook> UserDeferredBook { get; set; }
        public ApplicationDbContext() => Database.EnsureCreated();


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<UserDeferredBook>()
        //        .HasKey(ub => new { ub.UserId, ub.BookId });
        //    modelBuilder.Entity<UserReadedBook>()
        //        .HasKey(ur => new { ur.UserId, ur.BookId });
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=users_books.db");
        }
    }
}
