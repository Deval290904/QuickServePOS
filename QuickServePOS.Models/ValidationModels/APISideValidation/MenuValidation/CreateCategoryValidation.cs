using FluentValidation;
using QuickServePOS.Models.DTO.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels.APISideValidation.MenuValidation
{
    public class CreateCategoryValidation : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .NotEqual("string").WithMessage("Default value is not allowed.")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

            RuleFor(x => x.Description)
                .NotEqual("string").WithMessage("Default value is not allowed.")
                .MaximumLength(50).WithMessage("Description cannot exceed 50 characters.");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be a non-negative number.");
        }
    }
}
