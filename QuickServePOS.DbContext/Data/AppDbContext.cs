using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuickServePOS.Models.Entities;
using QuickServePOS.Models.Entities.Common;
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

            // =========================
            // ApplicationUser
            // =========================

            modelBuilder.Entity<ApplicationUser>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(x => x.UserProfile)
                .WithOne(x => x.User)
                .HasForeignKey<UserProfileEntity>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // UserProfile
            // =========================

            modelBuilder.Entity<UserProfileEntity>(entity =>
            {
                entity.HasQueryFilter(x => !x.IsDeleted);

                entity.Property(x => x.Gender)
                    .HasMaxLength(20);

                entity.Property(x => x.Address)
                    .HasMaxLength(500);
            });

            // =========================
            // RefreshToken
            // =========================

            modelBuilder.Entity<RefreshTokenEntity>(entity =>
            {
                entity.Property(x => x.RefreshToken)
                    .IsRequired();

                entity.HasIndex(x => x.RefreshToken)
                    .IsUnique();

                entity.HasIndex(x => x.UserId);

                entity.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================
            // Category
            // =========================

            modelBuilder.Entity<CategoryEntity>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.Description)
                    .HasMaxLength(500);

                entity.HasIndex(x => x.Name)
                    .IsUnique();

                entity.HasIndex(x => x.IsActive);

                entity.HasQueryFilter(x => !x.IsDeleted);
            });

            // =========================
            // MenuItem
            // =========================

            modelBuilder.Entity<MenuItemEntity>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(x => x.Description)
                    .HasMaxLength(500);

                entity.Property(x => x.HalfPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.FullPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.FoodType)
                    .HasConversion<int>();

                // INDEXES

                entity.HasIndex(x => x.CategoryId);

                entity.HasIndex(x => x.IsAvailable);

                entity.HasIndex(x => x.Is86d);

                entity.HasIndex(x => x.IsActive);

                entity.HasQueryFilter(x => !x.IsDeleted);

                entity.HasOne(x => x.Category)
                    .WithMany(x => x.MenuItems)
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}