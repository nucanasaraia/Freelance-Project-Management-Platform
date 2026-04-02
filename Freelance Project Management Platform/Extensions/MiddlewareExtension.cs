using Freelance_Project_Management_Platform.Middleware;

namespace Freelance_Project_Management_Platform.Extensions
{
    public static class MiddlewareExtension
    {
        public static void ConfigureMiddleware(this WebApplication app)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.MapControllers();
        }
    }
}
