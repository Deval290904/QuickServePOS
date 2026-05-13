using FluentValidation;
using QuickServePOS.Models.DTO.Order;
using QuickServePOS.Models.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels.APISideValidation.OrderValidation
{
    public class OrderCreateDtoValidation : AbstractValidator<OrderCreateDto>
    {
        public OrderCreateDtoValidation()
        {
            RuleFor(x => x.OrderType)
                 .IsInEnum()
                 .WithMessage("Invalid order type.");

            RuleFor(x => x.TableId)
                .NotNull().WithMessage("Table is required for dine-in orders.")
                .GreaterThan(0).WithMessage("Table ID must be greater than 0.")
                .When(x => x.OrderType == OrderType.DineIn)
                .WithMessage("Table is required for dine-in orders.");

            RuleFor(x => x.Notes)
                .NotEqual("string").WithMessage("Notes cannot be the default string value.")
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
        }
    }
}
