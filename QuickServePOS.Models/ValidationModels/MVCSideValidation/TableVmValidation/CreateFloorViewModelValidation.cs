using FluentValidation;
using QuickServePOS.Models.ViewModel.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels.MVCSideValidation.TableVmValidation
{
    public class CreateFloorViewModelValidation : AbstractValidator<CreateFloorViewModel>
    {
        public CreateFloorViewModelValidation()
        {
          
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Floor name is required.")
                .MaximumLength(100).WithMessage("Floor name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Floor Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    
    }
}
