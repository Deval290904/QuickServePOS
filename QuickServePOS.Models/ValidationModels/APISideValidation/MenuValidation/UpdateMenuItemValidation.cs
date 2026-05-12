using FluentValidation;
using QuickServePOS.Models.DTO.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels.APISideValidation.MenuValidation
{
    public class UpdateMenuItemValidation : AbstractValidator<UpdateMenuItemDto>
    {
        public  UpdateMenuItemValidation()
        {
            RuleFor(x => x.Id)
               .GreaterThan(0)
               .WithMessage("Invalid menu item ID.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("Please select a category.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Menu item name is required.")

                .NotEqual("string")
                .WithMessage("Default value is not allowed.")

                .MaximumLength(150)
                .WithMessage("Menu item name cannot exceed 150 characters.");

            RuleFor(x => x.Description)
                .NotEqual("string")
                .WithMessage("Default value is not allowed.")

                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters.");

            When(x => x.HalfPrice.HasValue, () =>
            {
                RuleFor(x => x.HalfPrice!.Value)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Half price must be greater than or equal to 0.");
            });

            RuleFor(x => x.FullPrice)
               .GreaterThan(0)
               .WithMessage("Full price must be greater than 0.");

            // FULL PRICE >= HALF PRICE
            When(x => x.HalfPrice.HasValue, () =>
            {
                RuleFor(x => x.FullPrice)
                    .GreaterThanOrEqualTo(x => x.HalfPrice!.Value)
                    .WithMessage("Full price must be greater than or equal to half price.");
            });

            RuleFor(x => x.PreparationTimeMinutes)
                .InclusiveBetween(1, 180)
                .WithMessage("Preparation time must be between 1 and 180 minutes.");

            RuleFor(x => x.FoodType)
                .IsInEnum()
                .WithMessage("Invalid food type.");

            RuleFor(x => x.ImageFile)
                .Must(file =>
                    file == null ||
                    new[]
                    {
                        ".jpg",
                        ".jpeg",
                        ".png",
                        ".webp"
                    }
                    .Contains(Path.GetExtension(file.FileName)
                    .ToLower()))
                .WithMessage("Only JPG, JPEG, PNG and WEBP files are allowed.");

            RuleFor(x => x.ImageFile)
                .Must(file =>
                    file == null ||
                    file.Length <= 2 * 1024 * 1024)
                .WithMessage("Image size cannot exceed 2 MB.");
        }
    }
    
}
