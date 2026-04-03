using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace Freelance_Project_Management_Platform.Extensions
{
    public static class RateLimitingExtension
    {
        public static void ConfigureRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                //general API limit
                options.AddFixedWindowLimiter("general", opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.PermitLimit = 60; 
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    opt.QueueLimit = 0;
                });

                //strict limit for auth endpoints
                options.AddFixedWindowLimiter("auth", opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.PermitLimit = 5; 
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    opt.QueueLimit = 0;
                });

                options.RejectionStatusCode = 429;
            });
        }
    }
}
