using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.DTOs;

namespace Freelance_Project_Management_Platform.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<List<UserDto>>> GetAllUsers();
        Task<ApiResponse<string>> BanUser(int userId);
    }
}
