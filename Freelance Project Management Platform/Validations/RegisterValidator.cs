
using FluentValidation;
using Freelance_Project_Management_Platform.Request;
using Microsoft.AspNetCore.Identity.Data;

public class RegisterValidator : AbstractValidator<RegistrationRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .Length(2, 50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .Matches("[A-Z]").WithMessage("Must contain uppercase")
            .Matches("[a-z]").WithMessage("Must contain lowercase")
            .Matches("[0-9]").WithMessage("Must contain number")
            .Matches("[^a-zA-Z0-9]").WithMessage("Must contain special character");
    }
}