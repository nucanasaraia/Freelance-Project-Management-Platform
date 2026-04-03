using FluentValidation;
using FluentValidation.AspNetCore;
using Freelance_Project_Management_Platform.Validations;

namespace Freelance_Project_Management_Platform.Extensions
{
    public static class ValidationExtension
    {
        public static void ConfigureValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<RegisterValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginValidator>();
            services.AddValidatorsFromAssemblyContaining<AddProjectValidator>();
            services.AddValidatorsFromAssemblyContaining<PasswordResetValidator>();
        }
    }
}