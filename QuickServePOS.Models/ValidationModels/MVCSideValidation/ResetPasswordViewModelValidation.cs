using FluentValidation;
using QuickServePOS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels.MVCSideValidation
{
    public class ResetPasswordViewModelValidation :AbstractValidator<ResetPasswordViewModel>
    {
        public ResetPasswordViewModelValidation()
        {

            RuleFor(x => x.NewPassword)
                  .NotEmpty().WithMessage("Password is required")
                  .Matches(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[\W_]).{6,}$")
                  .WithMessage("Password must be at least 6 characters and include 1 uppercase letter, 1 number, and 1 special character.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.NewPassword)
                .WithMessage("Passwords do not match");
        }
    }
}
