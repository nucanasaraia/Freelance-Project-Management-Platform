using Freelance_Project_Management_Platform.Helper;

namespace Freelance_Project_Management_Platform.Extensions
{
    public static class MappingExtension
    {
        public static void ConfigureMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}