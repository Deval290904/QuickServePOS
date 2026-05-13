using FluentValidation;
using QuickServePOS.Models.DTO.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels.APISideValidation.OrderValidation
{
    public class OrderUpdateDtoValidation : AbstractValidator<OrderUpdateDto>
    {
        public OrderUpdateDtoValidation()
        {
            RuleFor(x => x.Id)
               .GreaterThan(0).WithMessage("Invalid order ID.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid order status.");

            RuleFor(x => x.Notes)
                .NotEqual("string").WithMessage("Notes cannot be the default string value.")
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
                
        }
    }
}
