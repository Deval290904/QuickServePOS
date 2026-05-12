using FluentValidation;
using QuickServePOS.Models.DTO.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels.APISideValidation.AuthValidation
{
    public class ResetPasswordValidation : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .NotEqual("string").WithMessage("Not valid default value.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Token)
                .NotEqual("string").WithMessage("Not valid default value.")
                .NotEmpty().WithMessage("Token is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .Matches(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[\W_]).{6,}$")
                .WithMessage("Password must be at least 6 characters and include 1 uppercase letter, 1 number, and 1 special character.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required.")
                .Equal(x => x.NewPassword).WithMessage("Passwords do not match.");
        }
    }
}
