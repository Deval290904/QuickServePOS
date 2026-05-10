using Microsoft.AspNetCore.Http;
using QuickServePOS.Models.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.DTO.Menu
{
    public class UpdateMenuItemDto
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal? HalfPrice { get; set; }

        public decimal FullPrice { get; set; }

        public FoodType FoodType { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsActive { get; set; }

        public bool Is86d { get; set; }

        public string? ExistingImageUrl { get; set; }

        public int PreparationTimeMinutes { get; set; }

        // New Image
        public IFormFile? ImageFile { get; set; }
    }
}
