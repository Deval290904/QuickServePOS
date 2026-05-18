using QuickServePOS.Models.Entities.Common;
using QuickServePOS.Models.Entities.Enums;
using QuickServePOS.Models.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.Entities.Billing
{
    public class BillingEntity : BaseEntity
    {
        public int OrderId { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public decimal SubTotal { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal GrandTotal { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public bool IsPrinted { get; set; } = false;

        public DateTime BillDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual OrderEntity Order { get; set; }

        public virtual ICollection<PaymentEntity> Payments { get; set; }
            = new List<PaymentEntity>();
    }
}
