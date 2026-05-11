using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickServePOS.Models.Entities.Enums;

namespace QuickServePOS.Models.ViewModel.Menu
{
    public class CreateMenuItemViewModel
    {
        public int CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal? HalfPrice { get; set; }

        public decimal FullPrice { get; set; }

        public FoodType FoodType { get; set; }

        public bool IsAvailable { get; set; } = true;

        public bool IsActive { get; set; } = true;

        public bool Is86d { get; set; } = false;

        public int PreparationTimeMinutes { get; set; }

        public IFormFile? ImageFile { get; set; }

        // DROPDOWN
        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}
