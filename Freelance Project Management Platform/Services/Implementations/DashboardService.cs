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
        public DashboardService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ClientDashboardDto>> GetClientDashboard(int clientId)
        {
            var client = await _context.Users
                .Include(p => p.ClientProjects)
                .Include(p => p.Proposals)
                .FirstOrDefaultAsync(c => c.Id == clientId);

            if (client == null)
                return ApiResponseFactory.NotFound<ClientDashboardDto>("Client not found");

            var result = _mapper.Map<ClientDashboardDto>(client);
            return ApiResponseFactory.Success(result);
        }

        public async Task<ApiResponse<FreelancerDashboardDto>> GetFreelancerDashboard(int freelancerId)
        {
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
    }
}
