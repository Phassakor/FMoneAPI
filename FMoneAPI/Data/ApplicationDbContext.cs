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
        }

    }
}
