using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.Entities
{
    public class RefreshTokenEntity
    {
        public int Id { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; }=string.Empty;

        public ApplicationUser User { get; set; } = null;


    }
}
