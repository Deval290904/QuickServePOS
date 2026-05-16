using QuickServePOS.Models.Entities.Common;
using QuickServePOS.Models.Entities.Enums;
using QuickServePOS.Models.Entities.KOT;
using QuickServePOS.Models.Entities.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.Entities.Order
{
    public class OrderEntity : BaseEntity
    {
        public int? TableId { get; set; }

        public string OrderNo { get; set; } = null!;

        public OrderType OrderType { get; set; }

        public OrderStatus Status { get; set; }

        public decimal Subtotal { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Notes { get; set; }

        public DateTime? ConfirmedAt{ get; set; }

        public virtual RestaurantTableEntity? Table { get; set; }

        public virtual ICollection<OrderItemEntity> OrderItems { get; set; }
            = new List<OrderItemEntity>();

        public virtual ICollection<KOTEntity> KOTs { get; set; }= new List<KOTEntity>();
    }
}
