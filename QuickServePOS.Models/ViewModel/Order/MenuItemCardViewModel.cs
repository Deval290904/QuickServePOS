using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ViewModel.Order
{
    public class MenuItemCardViewModel
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public string Name { get; set; } = null!;

        public decimal FullPrice { get; set; }

        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
    }
}
