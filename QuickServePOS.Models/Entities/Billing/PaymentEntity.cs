using QuickServePOS.Models.Entities.Common;
using QuickServePOS.Models.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.Entities.Billing
{
    public class PaymentEntity : BaseEntity 
    {
        public int BillingId { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string? TransactionReference { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public PaymentStatus PaymentStatus { get; set; }

        // Navigation Property
        public virtual BillingEntity Billing { get; set; }
    }
}
