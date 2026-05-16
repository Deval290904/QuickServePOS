using QuickServePOS.Models.Entities.Enums;
using QuickServePOS.Models.Entities.Order;
using QuickServePOS.Models.Entities.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.Entities.KOT
{
    public class KOTEntity
    {
        public int KOTId { get; set; }

        public string KOTNumber { get; set; } = string.Empty;

        public int OrderId { get; set; }

        public int RestaurantTableId { get; set; }

        public KOTStatus Status { get; set; } = KOTStatus.New;

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        public DateTime? PreparingAt { get; set; }

        public DateTime? ReadyAt { get; set; }

        public DateTime? ServedAt { get; set; }

        public string? Notes { get; set; }

        public bool IsDeleted { get; set; }

        // Navigation Properties
 
        public virtual OrderEntity Order { get; set; } = null!;

        public virtual RestaurantTableEntity RestaurantTable { get; set; } = null!;

        public virtual ICollection<KOTItemEntity> KOTItems { get; set; }= new List<KOTItemEntity>();


    }
}
