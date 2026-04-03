using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Request;

namespace Freelance_Project_Management_Platform.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<PagedResult<UserDto>>> GetAllUsers(PaginationParams pagination);
        Task<ApiResponse<string>> BanUser(int userId);
    }
}
