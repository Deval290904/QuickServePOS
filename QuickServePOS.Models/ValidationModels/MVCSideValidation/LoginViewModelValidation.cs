using FluentValidation;
using QuickServePOS.Models.ViewModel.Authentication;

namespace QuickServePOS.Models.ValidationModels.MVCSideValidation
{
    public class LoginViewModelValidation : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidation() 
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .NotEqual("string").WithMessage("Not valid default value");
        }
    }
}
