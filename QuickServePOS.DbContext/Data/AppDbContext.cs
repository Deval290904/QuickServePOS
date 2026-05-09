using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuickServePOS.Models.Entities;
using QuickServePOS.Models.Entities.Menu;

namespace QuickServePOS.DbContextData.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserProfileEntity> UserProfiles { get; set; }

        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<MenuItemEntity> MenuItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(x => x.UserProfile)
                .WithOne(x => x.User)
                .HasForeignKey<UserProfileEntity>(x => x.UserId);

            modelBuilder.Entity<CategoryEntity>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                      .HasMaxLength(100)
                      .IsRequired();
                entity.HasIndex(x => x.Name)
                        .IsUnique();

                entity.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<MenuItemEntity>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                      .HasMaxLength(150)
                      .IsRequired();

                // INDEXES
                entity.HasIndex(x => x.CategoryId);

                entity.HasIndex(x => x.IsAvailable);

                entity.HasIndex(x => x.Is86d);

                entity.Property(x => x.HalfPrice)
                      .HasColumnType("decimal(18,2)");

                entity.Property(x => x.FullPrice)
                      .HasColumnType("decimal(18,2)");

                entity.HasQueryFilter(x => !x.IsDeleted);

                entity.HasOne(x => x.Category)
                      .WithMany(x => x.MenuItems)
                      .HasForeignKey(x => x.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}