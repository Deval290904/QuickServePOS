using FluentValidation;
using QuickServePOS.Models.ViewModel.Profile;

namespace QuickServePOS.Models.ValidationModels.MVCSideValidation
{
    public class UpdateStaffViewModelValidation : AbstractValidator<UpdateStaffViewModel>
    {
        public UpdateStaffViewModelValidation()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^[0-9]{10}$").WithMessage("Phone number must be exactly 10 digits.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Please select a role.");
        }
    }
}
