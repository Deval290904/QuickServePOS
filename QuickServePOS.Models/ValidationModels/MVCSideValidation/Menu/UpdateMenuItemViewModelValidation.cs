using FluentValidation;
using QuickServePOS.Models.ViewModel.Menu;

namespace QuickServePOS.Models.ValidationModels.MVCSideValidation.Menu
{
    public class UpdateMenuItemViewModelValidation : AbstractValidator<UpdateMenuItemViewModel>
    {
        public UpdateMenuItemViewModelValidation()
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

                .MaximumLength(150)
                .WithMessage("Menu item name cannot exceed 150 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters.");

            // OPTIONAL HALF PRICE
            When(x => x.HalfPrice.HasValue, () =>
            {
                RuleFor(x => x.HalfPrice!.Value)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Half price must be greater than or equal to 0.");
            });

            // FULL PRICE
            RuleFor(x => x.FullPrice)
                .NotEmpty()
                .WithMessage("Full price is required.")
                .GreaterThan(0)
                .WithMessage("Full price must be greater than 0.");

            // FULL >= HALF
            When(x => x.HalfPrice.HasValue, () =>
            {
                RuleFor(x => x.FullPrice)
                    .GreaterThanOrEqualTo(x => x.HalfPrice!.Value)
                    .WithMessage("Full price must be greater than or equal to half price.");
            });

            RuleFor(x => x.PreparationTimeMinutes)
                .InclusiveBetween(1, 180)
                .WithMessage("Preparation time must be between 1 and 180 minutes.");

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
                    .Contains(
                        Path.GetExtension(file.FileName)
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
