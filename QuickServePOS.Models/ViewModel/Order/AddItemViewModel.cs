using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ViewModel.Order
{
    public class AddItemViewModel
    {
        public int OrderId { get; set; }

        public int MenuItemId { get; set; }

        public int Quantity { get; set; }

        public string? SpecialInstruction { get; set; }
    }
}
