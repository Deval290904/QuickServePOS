using Microsoft.AspNetCore.Identity;
namespace QuickServePOS.Models.Entities.Auth
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool  IsEmailVerified { get; set; } = false;

        public UserProfileEntity? UserProfile { get; set; }

    }
}
