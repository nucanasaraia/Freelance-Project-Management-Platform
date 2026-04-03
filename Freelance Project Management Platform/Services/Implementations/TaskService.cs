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
        private readonly IUserLoggerService _logger;
        public TaskService(DataContext context, IMapper mapper, ICurrentUserService currentUser, IUserLoggerService logger)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
            _logger = logger;
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
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Unexpected error in {MethodName}", nameof(AssignTask));
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred");
            }
        }

        public async Task<ApiResponse<TaskDto>> CreateTask(int projectId, AddTask request)
        {
            try
            {
                var project = await _context.Projects
                    .FirstOrDefaultAsync(p => p.Id == projectId && p.ClientId == _currentUser.UserId);

                if (project == null)
                    return ApiResponseFactory.NotFound<TaskDto>("Project not found or access denied");

                var task = _mapper.Map<TaskItem>(request);
                task.ProjectId = projectId;
                task.CreatedAt = DateTime.UtcNow;
                task.Status = TASK_STATUS.TODO;

                await _context.TaskItems.AddAsync(task);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<TaskDto>(task);
                return ApiResponseFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Unexpected error in {MethodName}", nameof(CreateTask));
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
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Unexpected error in {MethodName}", nameof(DeleteTask));
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred");
            }
        }

        public async Task<ApiResponse<PagedResult<TaskDto>>> GetProjectTasks(int projectId, PaginationParams pagination)
        {
            try
            {
                var userId = _currentUser.UserId;

                var project = await _context.Projects
                    .FirstOrDefaultAsync(p => p.Id == projectId && p.ClientId == userId);

                if (project == null)
                {
                    return ApiResponseFactory.NotFound<PagedResult<TaskDto>>("Project not found");
                }

                if (pagination.Page <= 0) pagination.Page = 1;

                var query = _context.TaskItems
                    .AsNoTracking()
                    .Where(t => t.ProjectId == projectId);

                var totalCount = await query.CountAsync();

                var tasks = await query
                    .OrderByDescending(t => t.CreatedAt)
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                var result = new PagedResult<TaskDto>
                {
                    Items = _mapper.Map<List<TaskDto>>(tasks),
                    TotalCount = totalCount,
                    Page = pagination.Page,
                    PageSize = pagination.PageSize
                };

                return ApiResponseFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Unexpected error in {MethodName}", nameof(GetProjectTasks));
                return ApiResponseFactory.ServerError<PagedResult<TaskDto>>("Unexpected error occurred");
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
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Unexpected error in {MethodName}", nameof(UpdateTaskStatus));
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred");
            }
        }
    }
}
