using QuickServePOS.Models.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.Entities
{
    public class RefreshTokenEntity : BaseEntity
    {

        public string RefreshToken { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsRevoked { get; set; }

        public string UserId { get; set; }=string.Empty;

        public ApplicationUser User { get; set; } = null;


    }
}
