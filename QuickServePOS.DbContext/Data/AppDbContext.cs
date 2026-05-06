using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuickServePOS.Models.Entities;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(x => x.UserProfile)
                .WithOne(x => x.User)
                .HasForeignKey<UserProfileEntity>(x => x.UserId);
        }
    }
}