using FluentValidation;
using QuickServePOS.Models.ViewModel.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels.MVCSideValidation.Menu
{
    public class CreateCategoryViewModelValidation :AbstractValidator<CreateCategoryViewModel>
    {
        public CreateCategoryViewModelValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Category name is required.")

                .NotEqual("string")
                .WithMessage("Default value is not allowed.")

                .MaximumLength(50)
                .WithMessage("Category name cannot exceed 50 characters.");

            RuleFor(x => x.Description)
                .NotEqual("string")
                .WithMessage("Default value is not allowed.")

                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Display order must be greater than or equal to 0.");
        }
    }
}
