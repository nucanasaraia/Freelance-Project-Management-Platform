using FluentValidation;
using Freelance_Project_Management_Platform.Request;

namespace Freelance_Project_Management_Platform.Validations
{
    public class PasswordResetValidator : AbstractValidator<PasswordResetRequest>
    {
        public PasswordResetValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number");
        }
    }
}
