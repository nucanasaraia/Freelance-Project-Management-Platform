using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Enum;
using Freelance_Project_Management_Platform.Request;

namespace Freelance_Project_Management_Platform.Services.Interfaces
{
    public interface ITaskService
    {
        Task<ApiResponse<TaskDto>> CreateTask(int projectId, AddTask request);
        Task<ApiResponse<List<TaskDto>>> GetProjectTasks(int projectId);
        Task<ApiResponse<string>> AssignTask(int taskId, int assigneeId);
        Task<ApiResponse<string>> DeleteTask(int taskId);
        Task<ApiResponse<string>> UpdateTaskStatus(int taskId, TASK_STATUS status);
    }
}
