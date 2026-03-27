using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.DTOs;

namespace Freelance_Project_Management_Platform.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<ApiResponse<ClientDashboardDto>> GetClientDashboard(int clientId);
        Task<ApiResponse<FreelancerDashboardDto>> GetFreelancerDashboard(int freelancerId);
    }
}
