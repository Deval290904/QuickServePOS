using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuickServePOS.Models.Entities.Auth;
using QuickServePOS.Models.Entities.Common;
using QuickServePOS.Models.Entities.KOT;
using QuickServePOS.Models.Entities.Menu;
using QuickServePOS.Models.Entities.Order;
using QuickServePOS.Models.Entities.Table;

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

        public DbSet<FloorEntity> Floors { get; set; }

        public DbSet<RestaurantTableEntity> RestaurantTables { get; set; }

        public DbSet<TableMergeEntity> TableMerges { get; set; }

        public DbSet<OrderEntity> Orders { get; set; }

        public DbSet<OrderItemEntity> OrderItems { get; set; }

        public DbSet<KOTEntity> KOTs { get; set; }

        public DbSet<KOTItemEntity> KOTItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
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