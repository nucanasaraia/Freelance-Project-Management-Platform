using System.Text;

namespace Freelance_Project_Management_Platform.Extensions
{
    public static class JwtExtension
    {
        // Configure JWT authentication with proper null checks
        public static IServiceCollection ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            // Validate configuration values
            var jwtKey = configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT Key is not configured.");
            var jwtIssuer = configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer is not configured.");
            var jwtAudience = configuration["Jwt:Audience"]
                ?? throw new InvalidOperationException("JWT Audience is not configured.");

            var key = Encoding.UTF8.GetBytes(jwtKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    RoleClaimType = ClaimTypes.Role
                };
            });

            return services;
        }
    }
}
