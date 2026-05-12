using QuickServePOS.Models.Entities.Common;

namespace QuickServePOS.Models.Entities
{
    public class UserProfileEntity : BaseEntity
    {

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public DateTime? DOB { get; set; }

        public string? Gender { get; set; }

        public string? ProfileImagePath { get; set; }

        public DateTime? JoiningDate { get; set; }

        public string? Address { get; set; }
    }
}
