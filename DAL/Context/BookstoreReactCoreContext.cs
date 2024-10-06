using Microsoft.EntityFrameworkCore;
using Models;

namespace DAL.Context
{
    public class BookstoreReactCoreContext : DbContext
    {
        protected BookstoreReactCoreContext() { }
        public BookstoreReactCoreContext(DbContextOptions options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "User Admin",
                    UserName = "admin",
                    Password = PasswordHelper.ComputeHash("admin123"),
                    RefreshToken = null,
                    RefreshTokenExpiryTime = null
                }
            );
        }
    }
}
