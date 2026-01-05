using Microsoft.EntityFrameworkCore;
using eCommerceApi.Models;

namespace eCommerceApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
        
        public DbSet<Customer> Customers {get; set;}
        public DbSet<Product> Products {get; set;} 
        public DbSet<CartItem> CartItems {get; set;}
        public DbSet<Order> Orders {get; set;} 
        public DbSet<OrderItem> OrderItems {get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Price = 60000, Stock = 10 },
                new Product { Id = 2, Name = "Mouse", Price = 500, Stock = 50 },
                new Product { Id = 3, Name = "Keyboard", Price = 1500, Stock = 30 }
            );
        }
    }
}