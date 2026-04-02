using AutoMapper;
using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.Data;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Freelance_Project_Management_Platform.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<DashboardService> _logger;
        public DashboardService(DataContext context, IMapper mapper, ICurrentUserService currentUser, ILogger<DashboardService> logger)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<ApiResponse<ClientDashboardDto>> GetClientDashboard(int clientId)
        {
            try
            {
                if (_currentUser.UserId != clientId)
                    return ApiResponseFactory.Forbidden<ClientDashboardDto>("Access denied");

                var client = await _context.Users
                    .Include(p => p.ClientProjects)
                    .Include(p => p.Proposals)
                    .FirstOrDefaultAsync(c => c.Id == clientId);

                if (client == null)
                    return ApiResponseFactory.NotFound<ClientDashboardDto>("Client not found");

                var result = _mapper.Map<ClientDashboardDto>(client);
                return ApiResponseFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {MethodName}", nameof(GetClientDashboard));
                return ApiResponseFactory.ServerError<ClientDashboardDto>("Unexpected error occurred");
            }
        }

        public async Task<ApiResponse<FreelancerDashboardDto>> GetFreelancerDashboard(int freelancerId)
        {
            try
            {
                if (_currentUser.UserId != freelancerId)
                    return ApiResponseFactory.Forbidden<FreelancerDashboardDto>("Access denied");

                var freelancer = await _context.Users
                    .Include(p => p.AcceptedProjects)
                    .Include(p => p.Proposals)
                    .Include(p => p.AssignedTasks)
                    .FirstOrDefaultAsync(f => f.Id == freelancerId);

                if (freelancer == null)
                    return ApiResponseFactory.NotFound<FreelancerDashboardDto>("Freelancer not found");

                var result = _mapper.Map<FreelancerDashboardDto>(freelancer);
                return ApiResponseFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {MethodName}", nameof(GetFreelancerDashboard));
                return ApiResponseFactory.ServerError<FreelancerDashboardDto>("Unexpected error occurred");
            }
        }
    }
}
