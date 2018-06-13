using EShop.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Data
{
    public class EShopContext : DbContext
    {
        public EShopContext(DbContextOptions<EShopContext> options)
            : base(options)
        {
        }

        public DbSet<Commodity> Commodities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<BrowseHistory> BrowseHistories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<ShopCart> ShopCarts { get; set; }
        public DbSet<Thumb> Thumbs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Commodity>().ToTable("Commodity");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Tag>().ToTable("Tag");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<BrowseHistory>().ToTable("BrowseHistory");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Store>().ToTable("Store");

            modelBuilder.Entity<ShopCart>().ToTable("ShopCart")
                .HasKey(m => new { m.CommodityId, m.UserId });

            modelBuilder.Entity<Thumb>().ToTable("Thumb")
                .HasKey(m => new { m.CommodityId, m.FileName });
        }
    }
}
