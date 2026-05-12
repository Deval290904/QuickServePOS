using FluentValidation;
using QuickServePOS.Models.DTO.Floor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels.TableValidation
{
    public class FloorUpdateValidation : AbstractValidator<FloorUpdateDto>
    {
        public FloorUpdateValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .NotEqual("string").WithMessage("Not valid default value.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .NotEqual("string").WithMessage("Not valid default value.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
