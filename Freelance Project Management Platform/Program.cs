using Freelance_Project_Management_Platform.Configurations;
using Freelance_Project_Management_Platform.Data;
using Freelance_Project_Management_Platform.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

// Services
builder.Services.AddControllers();
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureServices();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureRateLimiting();
builder.Services.ConfigureValidation();
builder.Services.ConfigureMapping();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("SMTP"));

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    db.Database.Migrate();
}

// Middleware
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.ConfigureMiddleware();

app.Run();