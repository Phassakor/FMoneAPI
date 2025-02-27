using FMoneAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FMoneAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Banner> Banner { get; set; }
        public DbSet<Menus> Menus { get; set; }
        public DbSet<UserMenuPermission> UserMenuPermission { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsCategory> Newscategory { get; set; }
        public DbSet<NewsCategoryMapping> Newscategorymapping { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // ✅ ตั้งค่าความสัมพันธ์ระหว่าง User และ UserMenuPermissions
            modelBuilder.Entity<UserMenuPermission>()
                .HasOne(ump => ump.User)
                .WithMany(u => u.UserMenuPermissions)
                .HasForeignKey(ump => ump.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserMenuPermission>()
                .HasOne(ump => ump.Menu)
                .WithMany(m => m.UserMenuPermissions)
                .HasForeignKey(ump => ump.MenuId)
                .OnDelete(DeleteBehavior.Cascade);

            // ตั้งค่าคีย์หลักของ NewsCategoryMapping
            modelBuilder.Entity<NewsCategoryMapping>()
                .HasKey(ncm => new { ncm.NewsId, ncm.CategoryId });

            // สร้างความสัมพันธ์ Many-to-Many
            modelBuilder.Entity<NewsCategoryMapping>()
                .HasOne(ncm => ncm.News)
                .WithMany(n => n.CategoryMappings)
                .HasForeignKey(ncm => ncm.NewsId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NewsCategoryMapping>()
                .HasOne(ncm => ncm.NewsCategory)
                .WithMany(nc => nc.NewsMappings)
                .HasForeignKey(ncm => ncm.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
