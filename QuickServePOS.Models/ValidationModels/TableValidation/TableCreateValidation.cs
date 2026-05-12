using FluentValidation;
using QuickServePOS.Models.DTO.RestaurantTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels.TableValidation
{
    public class TableCreateValidation : AbstractValidator<TableCreateDto>
    {
        public TableCreateValidation()
        {
            RuleFor(x => x.FloorId)
                .GreaterThan(0).WithMessage("FloorId must be greater than 0.");

            RuleFor(x => x.TableNumber)
                .NotEmpty().WithMessage("Table number is required.")
                .NotEqual("string").WithMessage("Not valid default value.")
                .MaximumLength(20).WithMessage("Table number cannot exceed 20 characters.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Capacity must be greater than 0.")
                .LessThanOrEqualTo(50).WithMessage("Capacity cannot exceed 50.");
        }
    }
}
