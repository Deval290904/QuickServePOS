using FluentValidation;
using QuickServePOS.Models.DTO.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels.APISideValidation.OrderValidation
{
    public class OrderItemUpdateDtoValidation : AbstractValidator<OrderItemUpdateDto>
    {
        public OrderItemUpdateDtoValidation()
        {
            RuleFor(x => x.OrderItemId)
                .GreaterThan(0).WithMessage("Invalid order item ID.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Quantity must be less than or equal to 100.");

            RuleFor(x => x.SpecialInstruction)
                .NotEqual("string").WithMessage("Special instruction cannot be the default string value.")
                .MaximumLength(500).WithMessage("Special instruction must be less than or equal to 500 characters.");
                
        }
    }
}
