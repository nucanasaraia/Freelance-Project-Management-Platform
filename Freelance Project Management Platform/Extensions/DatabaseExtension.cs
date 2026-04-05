using Freelance_Project_Management_Platform.Data;
using Microsoft.EntityFrameworkCore;

namespace Freelance_Project_Management_Platform.Extensions
{
    public static class DatabaseExtension
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
