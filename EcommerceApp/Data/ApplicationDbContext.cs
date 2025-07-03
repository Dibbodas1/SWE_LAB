using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EcommerceApp.Models;

namespace EcommerceApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure Identity tables
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "AspNetUsers");
            });
            
            // Create admin and user roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER"
                }
            );

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic items and gadgets" },
                new Category { Id = 2, Name = "Clothing", Description = "Apparel and fashion items" },
                new Category { Id = 3, Name = "Books", Description = "Books and publications" },
                new Category { Id = 4, Name = "Home & Kitchen", Description = "Home and kitchen appliances" }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Smartphone",
                    Description = "Latest smartphone with advanced features",
                    Price = 699.99M,
                    ImageUrl = "/images/products/smartphone.jpg",
                    CategoryId = 1,
                    Stock = 50
                },
                new Product
                {
                    Id = 2,
                    Name = "Laptop",
                    Description = "High-performance laptop for work and gaming",
                    Price = 1299.99M,
                    ImageUrl = "/images/products/laptop.jpg",
                    CategoryId = 1,
                    Stock = 30
                },
                new Product
                {
                    Id = 3,
                    Name = "T-Shirt",
                    Description = "Comfortable cotton t-shirt",
                    Price = 19.99M,
                    ImageUrl = "/images/products/tshirt.jpg",
                    CategoryId = 2,
                    Stock = 100
                },
                new Product
                {
                    Id = 4,
                    Name = "Jeans",
                    Description = "Classic blue jeans",
                    Price = 49.99M,
                    ImageUrl = "/images/products/jeans.jpg",
                    CategoryId = 2,
                    Stock = 75
                },
                new Product
                {
                    Id = 5,
                    Name = "Novel",
                    Description = "Bestselling fiction novel",
                    Price = 14.99M,
                    ImageUrl = "/images/products/novel.jpg",
                    CategoryId = 3,
                    Stock = 60
                },
                new Product
                {
                    Id = 6,
                    Name = "Coffee Maker",
                    Description = "Automatic coffee maker for home use",
                    Price = 89.99M,
                    ImageUrl = "/images/products/coffeemaker.jpg",
                    CategoryId = 4,
                    Stock = 40
                }
            );
        }
    }
}