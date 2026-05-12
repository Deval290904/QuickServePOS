using FluentValidation;
using QuickServePOS.Models.ViewModel.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels.MVCSideValidation.AuthVmValidation
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(3);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^[0-9]{10}$")
                .WithMessage("Phone number must be 10 digits");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .Matches(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[\W_]).{6,}$")
                .WithMessage("Password must be at least 6 characters and include 1 uppercase letter, 1 number, and 1 special character.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match");
        }
    }
}
