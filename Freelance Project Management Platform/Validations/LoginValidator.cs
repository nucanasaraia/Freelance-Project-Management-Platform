using FluentValidation;
using Freelance_Project_Management_Platform.Request;

namespace Freelance_Project_Management_Platform.Validations
{
    public class LoginValidator : AbstractValidator<LogInRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
