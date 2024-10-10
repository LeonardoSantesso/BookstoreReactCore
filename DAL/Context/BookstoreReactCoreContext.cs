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
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("books");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

                entity.Property(e => e.Title).HasColumnName("title").IsRequired();
                entity.Property(e => e.Author).HasColumnName("author").IsRequired();
                entity.Property(e => e.Price).HasColumnName("price").HasColumnType("decimal(18,2)");
                entity.Property(e => e.LaunchDate).HasColumnName("launch_date").HasColumnType("datetime");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName).HasColumnName("first_name").IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).HasColumnName("last_name").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Date).HasColumnName("date").HasColumnType("datetime").IsRequired();
                
                entity.HasMany(e => e.OrderItems)
                    .WithOne(e => e.Order)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("order_items");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

                entity.Property(e => e.Quantity).HasColumnName("quantity").HasDefaultValue(0);
                entity.Property(e => e.Total).HasColumnName("total").HasColumnType("decimal(18,2)");
                entity.Property(e => e.Price).HasColumnName("price").HasColumnType("decimal(18,2)");
                entity.Property(e => e.OrderId).HasColumnName("order_id").IsRequired();

                entity.HasOne(e => e.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("persons");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName).HasColumnName("first_name").IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).HasColumnName("last_name").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Address).HasColumnName("address").HasMaxLength(200);
                entity.Property(e => e.Gender).HasColumnName("gender").HasMaxLength(10);
                entity.Property(e => e.Enabled).HasColumnName("enabled").IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

                entity.Property(e => e.UserName).HasColumnName("user_name").IsRequired();
                entity.Property(e => e.FullName).HasColumnName("full_name").IsRequired();
                entity.Property(e => e.Password).HasColumnName("password").IsRequired();
            });
            
            // Populate database
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "User Admin",
                    UserName = "admin",
                    Password = PasswordHelper.ComputeHash("admin123"),
                }
            );
        }
    }
}
