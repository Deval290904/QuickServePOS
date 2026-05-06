using FluentValidation;
using QuickServePOS.Models.DTO.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels
{
    public class UpdateProfileValidation : AbstractValidator<UpdateProfileDto>
    {
        public UpdateProfileValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .NotEqual("string").WithMessage("Name cannot be 'string'")
                .MaximumLength(25);

            RuleFor(x => x.PhoneNumber)
               .NotEmpty().WithMessage("Phone is required")
               .Matches(@"^[0-9]{10}$").WithMessage("Phone must be 10 digits");


            RuleFor(x => x.Address)
                .MaximumLength(250);

            RuleFor(x => x.Gender)
                .Must(x =>
                    x == null ||
                    x == "Male" ||
                    x == "Female")
                .WithMessage("Invalid gender");
        }
    }
}
