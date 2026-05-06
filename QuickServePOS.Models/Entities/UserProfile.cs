namespace QuickServePOS.Models.Entities
{
    public class UserProfile
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public DateTime? DOB { get; set; }

        public string? Gender { get; set; }

        public string? ProfileImagePath { get; set; }

        public DateTime? JoiningDate { get; set; }

        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
