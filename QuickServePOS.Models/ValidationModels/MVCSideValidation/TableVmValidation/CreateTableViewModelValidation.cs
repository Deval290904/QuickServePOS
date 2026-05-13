using FluentValidation;
using QuickServePOS.Models.ViewModel.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels.MVCSideValidation.TableVmValidation
{
    public class CreateTableViewModelValidation : AbstractValidator<CreateTableViewModel>
    {
        public CreateTableViewModelValidation()
        {
            RuleFor(x => x.FloorId)
                 .GreaterThan(0).WithMessage("Please select floor.");

            RuleFor(x => x.TableNumber)
                .NotEmpty().WithMessage("Table number is required.")
                .MaximumLength(20).WithMessage("Table number maximum 20 characters.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Capacity must be greater than 0.")
                .LessThanOrEqualTo(20).WithMessage("Capacity cannot exceed 20.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid table status.");
        }
    }
}
