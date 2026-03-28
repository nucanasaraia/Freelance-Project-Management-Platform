using FluentValidation;
using FluentValidation.AspNetCore;

namespace StudentCourseManagement.Extensions
{
    public static class ValidationExtension
    {
        public static void ConfigureValidation(this IServiceCollection services)
        {
           services.AddValidatorsFromAssemblyContaining<RegisterValidator>();
        }
    }
}