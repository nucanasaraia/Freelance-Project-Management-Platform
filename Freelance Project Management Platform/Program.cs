using Freelance_Project_Management_Platform.Configurations;
using Freelance_Project_Management_Platform.Extensions;
using Serilog;
using StudentCourseManagement.Extensions;

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
builder.Services.ConfigureValidation();
builder.Services.ConfigureMapping();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("SMTP"));

var app = builder.Build();


// Middleware
app.UseAuthentication();
app.UseAuthorization();

app.ConfigureMiddleware();

app.Run();