using AutoMapper;
using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.Data;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Enum;
using Freelance_Project_Management_Platform.Models;
using Freelance_Project_Management_Platform.Request;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Freelance_Project_Management_Platform.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        public TaskService(DataContext context, IMapper mapper, ICurrentUserService currentUser)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<ApiResponse<string>> AssignTask(int taskId, int assigneeId)
        {
            try
            {
                var task = await _context.TaskItems
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.Project.ClientId == _currentUser.UserId);

                if (task == null)
                {
                    return ApiResponseFactory.NotFound<string>("Task not found or access denied");
                }

                task.AssignedToUserId = assigneeId;
                await _context.SaveChangesAsync();

                return ApiResponseFactory.Success("Task assigned successfully");
            }
            catch
            {
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred");
            }
        }

        public async Task<ApiResponse<TaskDto>> CreateTask(int projectId, AddTask request)
        {
            try
            {
                var task = _mapper.Map<TaskItem>(request);

                task.ProjectId = projectId;
                task.CreatedAt = DateTime.UtcNow;
                task.Status = TASK_STATUS.TODO;

                await _context.TaskItems.AddAsync(task);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<TaskDto>(task);

                return ApiResponseFactory.Success(result);
            }
            catch
            {
                return ApiResponseFactory.ServerError<TaskDto>("Unexpected error occurred");
            }
        }

        public async Task<ApiResponse<string>> DeleteTask(int taskId)
        {
            try
            {
                var task = await _context.TaskItems
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId &&
                (t.AssignedToUserId == _currentUser.UserId || t.Project.ClientId == _currentUser.UserId));

                if (task == null)
                {
                    return ApiResponseFactory.NotFound<string>("Task not found");
                }

                _context.TaskItems.Remove(task);
                await _context.SaveChangesAsync();

                return ApiResponseFactory.Success("Task deleted successfully");
            }
            catch
            {
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred");
            }
        }

        public async Task<ApiResponse<List<TaskDto>>> GetProjectTasks(int projectId)
        {
            try
            {
                var project = await _context.Projects
                    .FirstOrDefaultAsync(p => p.Id == projectId && p.ClientId == _currentUser.UserId);

                if (project == null)
                    return ApiResponseFactory.NotFound<List<TaskDto>>("Project not found or access denied");

                var tasks = await _context.TaskItems
                    .Where(t => t.ProjectId == projectId)
                    .ToListAsync();


                if (!tasks.Any())
                {
                    return ApiResponseFactory.Success(new List<TaskDto>());
                }

                var result = _mapper.Map<List<TaskDto>>(tasks);
                return ApiResponseFactory.Success(result);
            }
            catch
            {
                return ApiResponseFactory.ServerError<List<TaskDto>>("Unexpected error occurred");
            }
        }

        public async Task<ApiResponse<string>> UpdateTaskStatus(int taskId, TASK_STATUS status)
        {
            try
            {
                var task = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == taskId && t.AssignedToUserId == _currentUser.UserId);

                if (task == null)
                {
                    return ApiResponseFactory.NotFound<string>("Task not found or access denied");
                }

                if (task.Status == TASK_STATUS.DONE)
                {
                    return ApiResponseFactory.BadRequest<string>("Task is already completed");
                }

                task.Status = status;
                await _context.SaveChangesAsync();

                return ApiResponseFactory.Success("Task updated successfully");
            }
            catch
            {
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred");
            }
        }
    }
}
