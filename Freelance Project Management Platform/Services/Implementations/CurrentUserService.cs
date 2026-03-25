using Freelance_Project_Management_Platform.Services.Interfaces;
using System.Security.Claims;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId =>
        int.Parse(_httpContextAccessor.HttpContext!
            .User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
}