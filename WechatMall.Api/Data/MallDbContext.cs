using Microsoft.EntityFrameworkCore;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Data
{
    public class MallDbContext : DbContext
    {
        public MallDbContext(DbContextOptions<MallDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ShippingFare> ShippingFares { get; set; }
        public DbSet<SiteConfig> Configs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.CategoryID).IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.ProductID).IsUnique();
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.CategoryID);

            modelBuilder.Entity<ProductImage>()
                .HasIndex(p => p.Guid).IsUnique();
            modelBuilder.Entity<ProductImage>()
                .HasIndex(p => p.ProductID);

            modelBuilder.Entity<SiteConfig>()
                .HasIndex(s => s.Key).IsUnique();

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(p => p.Products)
                .HasPrincipalKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Product>()
                .HasOne(p=>p.ShippingFare)
                .WithMany(s=>s.Products)
                .HasForeignKey(p => p.ShippingFareID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<ProductImage>()
                .HasOne(p => p.Product)
                .WithMany(p => p.Images)
                .HasPrincipalKey(p => p.ProductID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(i => i.Order)
                .HasPrincipalKey(o => o.OrderID)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
