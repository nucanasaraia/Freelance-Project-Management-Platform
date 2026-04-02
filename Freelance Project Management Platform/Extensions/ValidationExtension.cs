using FluentValidation;
using FluentValidation.AspNetCore;

namespace Freelance_Project_Management_Platform.Extensions
{
    public static class ValidationExtension
    {
        public static void ConfigureValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<RegisterValidator>();
        }
    }
}