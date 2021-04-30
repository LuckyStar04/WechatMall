using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShippingAddr> ShippingAddrs { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Coupon_User> Coupon_Users { get; set; }

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

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderID).IsUnique();

            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShippingAddr)
                .WithMany(s => s.Orders)
                .HasForeignKey(s => s.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Product)
                .WithMany(p => p.OrderItems)
                .HasPrincipalKey(p => p.ProductID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserID).IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.OpenID).IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasPrincipalKey(p => p.UserID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<ShippingAddr>()
                .HasOne(s => s.User)
                .WithMany(u => u.ShippingAddrs)
                .HasPrincipalKey(u => u.UserID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Coupon_User>()
                .HasKey(c => new { c.CouponID, c.UserID });

            modelBuilder.Entity<Coupon_User>()
                .HasOne(c => c.Coupon)
                .WithMany(c => c.Users)
                .HasForeignKey(c => c.CouponID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Coupon_User>()
                .HasOne(c => c.User)
                .WithMany(u => u.Coupons)
                .HasPrincipalKey(c => c.UserID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //DefaultValue
            modelBuilder.Entity<Category>()
                .Property(c => c.IsShown).HasDefaultValue(true);
            modelBuilder.Entity<Category>()
                .Property(c => c.IsDeleted).HasDefaultValue(false);
            modelBuilder.Entity<Order>()
                .Property(c => c.IsDeleted).HasDefaultValue(false);
            modelBuilder.Entity<Product>()
                .Property(c => c.Recommend).HasDefaultValue(0);
            modelBuilder.Entity<Product>()
                .Property(c => c.OnSale).HasDefaultValue(true);
            modelBuilder.Entity<Product>()
                .Property(c => c.IsDeleted).HasDefaultValue(false);
            modelBuilder.Entity<ShippingFare>()
                .Property(c => c.IsDeleted).HasDefaultValue(false);
        }
    }
}
