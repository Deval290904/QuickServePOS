using FluentValidation;
using QuickServePOS.Models.ViewModel.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels.MVCSideValidation.OrderVmValidation
{
    public class AddItemViewModelValidation :AbstractValidator<AddItemViewModel>
    {
        public AddItemViewModelValidation()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("OrderId must be greater than 0.");

            RuleFor(x => x.MenuItemId)
                .GreaterThan(0).WithMessage("MenuItemId must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.")
                .LessThanOrEqualTo(50).WithMessage("Quantity must be less than or equal to 50.");

            RuleFor(x => x.SpecialInstruction)
                .MaximumLength(500).WithMessage("SpecialInstruction must be less than or equal to 500 characters.");
        }
    }
}
