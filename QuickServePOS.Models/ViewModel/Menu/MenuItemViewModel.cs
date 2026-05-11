using QuickServePOS.Models.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ViewModel.Menu
{
    public class MenuItemViewModel
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal? HalfPrice { get; set; }

        public decimal FullPrice { get; set; }

        public FoodType FoodType { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsActive { get; set; }

        public bool Is86d { get; set; }

        public string? ImageUrl { get; set; }

        public int PreparationTimeMinutes { get; set; }
    }
}
