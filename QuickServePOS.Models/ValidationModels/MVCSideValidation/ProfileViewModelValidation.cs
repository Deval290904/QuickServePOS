using FluentValidation;
using QuickServePOS.Models.ViewModel;

namespace QuickServePOS.Models.ValidationModels.MVCSideValidation
{
    public class ProfileViewModelValidation :AbstractValidator<ProfileViewModel>
    {
        public ProfileViewModelValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(3);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^[0-9]{10}$");

            RuleFor(x => x.Address)
                .MaximumLength(250);
        }
    }
}
