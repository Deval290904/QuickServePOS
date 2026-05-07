using FluentValidation;
using QuickServePOS.Models.DTO.Admin;

namespace QuickServePOS.Models.ValidationModels
{
    public class CreateStaffValidation : AbstractValidator<CreateStaffAccountDto>
    {
        public CreateStaffValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .NotEqual("string").WithMessage("Name cannot be 'string'")
                .MaximumLength(25);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .Matches(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[\W_]).{6,}$")
                .WithMessage("Password must be at least 6 characters and include 1 uppercase letter, 1 number, and 1 special character.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone is required")
                .Matches(@"^[0-9]{10}$").WithMessage("Phone must be 10 digits");


            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required")
                .Must(role => new[] { "Owner", "Waiter", "Cashier", "KitchenStaff" }.Contains(role))
                .WithMessage("Invalid role selected");

           RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match");
        }
    }
}
