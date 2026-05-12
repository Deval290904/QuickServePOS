using FluentValidation;
using QuickServePOS.Models.ViewModel.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels.MVCSideValidation.TableVmValidation
{
    public class UpdateFloorViewModelValidation : AbstractValidator<UpdateFloorViewModel>
    {
        public UpdateFloorViewModelValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid floor ID.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Floor name is required.")
                .MaximumLength(100).WithMessage("Floor name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
