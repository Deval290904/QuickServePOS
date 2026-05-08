using FluentValidation;
using QuickServePOS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels.MVCSideValidation
{
    public class ProfileUpdateViewModelValidation : AbstractValidator<ProfileUpdateViewModel>
    {
        public ProfileUpdateViewModelValidation()
        {
            RuleFor(x => x.Name)
                   .NotEmpty().WithMessage("Name is required")
                   .MinimumLength(3).WithMessage("Name must be at least 3 characters")
                   .MaximumLength(50).WithMessage("Name must be at most 50 characters");

            RuleFor(x => x.PhoneNumber)
                        .NotEmpty().WithMessage("Phone number is required")
                        .Matches(@"^[0-9]{10}$")
                        .WithMessage("Phone number must be 10 digits");

            RuleFor(x => x.DOB)
                .LessThan(DateTime.Today)
                .When(x => x.DOB.HasValue)
                .WithMessage("DOB cannot be a future date");

            RuleFor(x => x.Address)
                        .MaximumLength(100).WithMessage("Address must be at most 100 characters");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required")
                        .Must(x =>
                            string.IsNullOrEmpty(x)
                            || x == "Male"
                            || x == "Female")
                        .WithMessage("Invalid gender");
        }
    }
}
