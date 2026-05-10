using Microsoft.AspNetCore.Http;
using QuickServePOS.Models.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.DTO.Menu
{
    public class CreateMenuItemDto
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

        // Image Upload
        public IFormFile? ImageFile { get; set; }
    }
}
