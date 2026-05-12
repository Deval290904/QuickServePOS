using FluentValidation;
using QuickServePOS.Models.ViewModel.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels.MVCSideValidation.AuthVmValidation
{
    public class ForgotPasswordViewModelValidation :AbstractValidator<ForgotPasswordViewModel>
    {
        public ForgotPasswordViewModelValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
