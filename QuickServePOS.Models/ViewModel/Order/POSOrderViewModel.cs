using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ViewModel.Order
{
    public class POSOrderViewModel
    {
        public OrderDetailsViewModel Order { get; set; } = new ();

        public List<MenuCategoryViewModel> Categories
        { get; set; }
            = new();

        public List<MenuItemCardViewModel> MenuItems
        { get; set; }
            = new();
    }
}
