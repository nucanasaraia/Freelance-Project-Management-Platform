using FluentValidation;
using Freelance_Project_Management_Platform.Request;

namespace Freelance_Project_Management_Platform.Validations
{
    public class AddProjectValidator : AbstractValidator<AddProject>
    {
        public AddProjectValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters");

            RuleFor(x => x.Budget)
                .GreaterThan(0).WithMessage("Budget must be greater than 0");

            RuleFor(x => x.Deadline)
                .GreaterThan(DateTime.UtcNow).WithMessage("Deadline must be in the future");
        }
    }
}
