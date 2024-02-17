using Ecommerce.Models;
using Ecommerce2.Dtos;
using Ecommerce2.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce2.Data
{
    public class DataContextEf : DbContext
    {
        private readonly IConfiguration _config;

        public DataContextEf(IConfiguration configuration)
        {
            _config = configuration;
        }
        public DbSet<Product> products { get; set; }
        public DbSet<Ad> ads { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<ShippingAddress> shippingAddresses { get; set; }
        public DbSet<OrderItem> orderItems { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<UserForLoginConfirmationDto> userForLoginConfirmationDtos { get; set; }
        public DbSet<ProductCategory> productCategories { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("EcommerceSchema");
            modelBuilder.Entity<Product>().ToTable("Product", "EcommerceSchema").HasKey(p => p.ProductId);
            modelBuilder.Entity<Ad>().ToTable("Anunt", "EcommerceSchema").HasKey(p => p.AnuntId);
            modelBuilder.Entity<Order>().ToTable("Order", "EcommerceSchema").HasKey(k => k.OrderId);
            modelBuilder.Entity<Order>().HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId);
            modelBuilder.Entity<User>().ToTable("User", "EcommerceSchema").HasKey(k => k.UserId);
            modelBuilder.Entity<ShippingAddress>().ToTable("ShippingAddress", "EcommerceSchema").HasKey(k => k.AddressId);
            modelBuilder.Entity<ShippingAddress>().HasOne(o => o.User).WithMany(u => u.ShippingAddresses).HasForeignKey(o => o.UserId); //For that specific user can have many shipping addresses
            modelBuilder.Entity<OrderItem>().HasOne(o => o.Order).WithMany(u => u.Items).HasForeignKey(o => o.OrderId);
            modelBuilder.Entity<OrderItem>().ToTable("OrderItem", "EcommerceSchema").HasKey(k => new { k.OrderId, k.ProductId });
            modelBuilder.Entity<Category>().ToTable("Category", "EcommerceSchema").HasKey(k => k.CategoryId);
            modelBuilder.Entity<UserForLoginConfirmationDto>().ToTable("Auth", "EcommerceSchema");
            modelBuilder.Entity<Ad>().HasOne(ad => ad.Product).WithOne(product => product.Ads).HasForeignKey<Ad>(ad => ad.ProductId);
            modelBuilder.Entity<ProductCategory>().HasOne(pc => pc.Product).WithMany(p => p.ProductCategory).HasForeignKey(pc => pc.ProductId);
            modelBuilder.Entity<ProductCategory>().HasOne(pc => pc.Category).WithMany(c => c.ProductCategory).HasForeignKey(pc => pc.CategoryId);
            modelBuilder.Entity<ProductCategory>().ToTable("ProductCategory", "EcommerceSchema").HasKey(p => p.ProductCategoryId);
        }
    }
}