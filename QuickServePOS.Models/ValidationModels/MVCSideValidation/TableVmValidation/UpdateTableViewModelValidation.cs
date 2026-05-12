using FluentValidation;
using QuickServePOS.Models.ViewModel.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels.MVCSideValidation.TableVmValidation
{
    public class UpdateTableViewModelValidation : AbstractValidator<UpdateTableViewModel>
    {
        public UpdateTableViewModelValidation() 
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid table ID.");

            RuleFor(x => x.FloorId)
                .GreaterThan(0).WithMessage("Please select floor.");

            RuleFor(x => x.TableNumber)
                .NotEmpty().WithMessage("Table number is required.")
                .MaximumLength(20).WithMessage("Table number cannot exceed 20 characters.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Capacity must be greater than 0.")
                .LessThanOrEqualTo(50).WithMessage("Capacity cannot exceed 50.");
        }
    }
}
