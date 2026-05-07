using FluentValidation;
using QuickServePOS.Models.DTO.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels
{
    public class ForgotPasswordValidation :AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
