using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Request;

namespace Freelance_Project_Management_Platform.Services.Interfaces
{
    public interface IProjectService
    {
        Task<ApiResponse<ProjectDto>> CreateProject(AddProject request);
        Task<ApiResponse<List<ProjectDto>>> GetAllProjects();
        Task<ApiResponse<ProjectDto>> GetProject(int id);
        Task<ApiResponse<string>> UpdateProject(int projectId, AddProject request);
        Task<ApiResponse<string>> DeleteProject(int projectId);
        Task<ApiResponse<string>> MarkAsCompleted(int projectId);
        Task<ApiResponse<string>> MarkAsPaid(int projectId);
    }
}
