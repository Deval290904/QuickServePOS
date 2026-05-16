using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ViewModel.KOT
{
    public class KOTItemViewModel
    {
        public int KOTItemId { get; set; }

        public string MenuItemName { get; set; }= string.Empty;

        public int Quantity { get; set; }

        public string Status { get; set; }= string.Empty;

        public string? SpecialInstruction { get; set; }
    }
}
