using Freelance_Project_Management_Platform.Services.Implementations;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Cors.Infrastructure;
using StudentCourseManagement.Services.Implementations;

namespace Freelance_Project_Management_Platform.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IProposalService, ProposalService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IUserLoggerService, UserLoggerService>();
        }
    }
}
