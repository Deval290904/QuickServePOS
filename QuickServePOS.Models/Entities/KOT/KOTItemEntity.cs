using QuickServePOS.Models.Entities.Enums;
using QuickServePOS.Models.Entities.Menu;
using QuickServePOS.Models.Entities.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.Entities.KOT
{
    public class KOTItemEntity
    {
        public int KOTItemId { get; set; }

        public int KOTId { get; set; }

        public int OrderItemId { get; set; }

        public int MenuItemId { get; set; }

        public int Quantity { get; set; }

        public int PreparedQuantity { get; set; }

        public KitchenItemStatus Status { get; set; }= KitchenItemStatus.Pending;

        public string? SpecialInstruction { get; set; }

        public DateTime? PreparingAt { get; set; }

        public DateTime? ReadyAt { get; set; }

        public bool IsDeleted { get; set; }

      //Navigation Properties

        public virtual KOTEntity KOT { get; set; } = null!;

        public virtual OrderItemEntity OrderItem { get; set; } = null!;

        public virtual MenuItemEntity MenuItem { get; set; } = null!;

    }
}
