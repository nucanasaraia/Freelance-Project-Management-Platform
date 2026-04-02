using AutoMapper;
using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.Data;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Enum;
using Freelance_Project_Management_Platform.Models;
using Freelance_Project_Management_Platform.Request;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Freelance_Project_Management_Platform.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        private readonly IUserLoggerService _logger;
        public ProjectService(DataContext context, IMapper mapper, ICurrentUserService currentUser, IUserLoggerService logger)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
            _logger = logger;
        }

        public  async Task<ApiResponse<ProjectDto>> CreateProject(AddProject request)
        {
            try
            {
                var userId = _currentUser.UserId;

                var newProject = _mapper.Map<Project>(request);

                newProject.ClientId = userId;
                newProject.CreatedAt = DateTime.UtcNow;

                await _context.Projects.AddAsync(newProject);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<ProjectDto>(newProject);
                return ApiResponseFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Unexpected error in {MethodName}", nameof(CreateProject));
                return ApiResponseFactory.ServerError<ProjectDto>("Unexpected error occurred");
            }
        }

        public async Task<ApiResponse<string>> DeleteProject(int projectId)
        {
            try
            {
                var userId = _currentUser.UserId;

                var project = await _context.Projects
                    .FirstOrDefaultAsync(p => p.Id == projectId && p.ClientId == userId);

                if (project == null)
                {
                    return ApiResponseFactory.NotFound<string>("Project not found or access denied");
                }

                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();

                return ApiResponseFactory.Success("Project deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Unexpected error in {MethodName}", nameof(DeleteProject));
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred");
            }
        }

        public async Task<ApiResponse<string>> UpdateProject(int projectId, AddProject request)
        {
            try
            {
                var userId = _currentUser.UserId;
                var project = await _context.Projects
                                .FirstOrDefaultAsync(p => p.Id == projectId && p.ClientId == userId);

                if (project == null)
                {
                    return ApiResponseFactory.NotFound<string>("Project not found or access denied");
                }

                _mapper.Map(request, project);
                await _context.SaveChangesAsync();

                return ApiResponseFactory.Success("Project updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Unexpected error in {MethodName}", nameof(UpdateProject));
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred");
            }
        }

        public async Task<ApiResponse<List<ProjectDto>>> GetAllProjects()
        {
            try
            {
                var userId = _currentUser.UserId;

                var projects = await _context.Projects
                    .Where(p => p.ClientId == userId)
                    .ToListAsync();

                if (!projects.Any())
                {
                    return ApiResponseFactory.Success(new List<ProjectDto>());
                }

                var result = _mapper.Map<List<ProjectDto>>(projects);
                return ApiResponseFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Unexpected error in {MethodName}", nameof(GetAllProjects));
                return ApiResponseFactory.ServerError<List<ProjectDto>>("Unexpected error occurred");
            }
        }

        public async Task<ApiResponse<ProjectDto>> GetProject(int projectId)
        {
            try
            {
                var userId = _currentUser.UserId;
                var project = await _context.Projects
                    .FirstOrDefaultAsync(p => p.Id == projectId && p.ClientId == userId);

                if (project == null)
                {
                    return ApiResponseFactory.NotFound<ProjectDto>("Project not found or access denied");
                }

                var result = _mapper.Map<ProjectDto>(project);
                return ApiResponseFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Unexpected error in {MethodName}", nameof(GetProject));
                return ApiResponseFactory.ServerError<ProjectDto>("Unexpected error occurred");
            }
        }

        public async Task<ApiResponse<string>> MarkAsCompleted(int projectId)
        {
            try
            {
                var userId = _currentUser.UserId;
                var project = await _context.Projects
                    .FirstOrDefaultAsync(p => p.Id == projectId && p.ClientId == userId);

                if (project == null)
                {
                    return ApiResponseFactory.NotFound<string>("Project not found or access denied");
                }

                if (project.Status == PROJECT_STATUS.COMPLETED)
                    return ApiResponseFactory.BadRequest<string>("Project is already completed");

                project.Status = PROJECT_STATUS.COMPLETED;
                await _context.SaveChangesAsync();

                return ApiResponseFactory.Success("Project has been marked as Completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Unexpected error in {MethodName}", nameof(MarkAsCompleted));
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred");
            }
        }

        public async Task<ApiResponse<string>> MarkAsPaid(int projectId)
        {
            try
            {
                var project = await _context.Projects.FindAsync(projectId);

                if (project == null)
                    return ApiResponseFactory.NotFound<string>("Project not found");

                if (project.IsPaid)
                    return ApiResponseFactory.BadRequest<string>("Project is already marked as paid");

                project.IsPaid = true;
                await _context.SaveChangesAsync();

                return ApiResponseFactory.Success("Project has been marked as Paid");
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Unexpected error in {MethodName}", nameof(MarkAsPaid));
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred");
            }
        }
    }
}
