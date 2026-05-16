using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ViewModel.KOT
{
    public class KitchenQueueViewModel
    {
        public int KOTId { get; set; }

        public string KOTNumber { get; set; } = string.Empty;

        public string TableName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime GeneratedAt { get; set; }

        public int TotalItems { get; set; }
    }
}
