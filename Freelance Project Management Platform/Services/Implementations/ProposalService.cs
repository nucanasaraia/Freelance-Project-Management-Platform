using AutoMapper;
using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.Data;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Enum;
using Freelance_Project_Management_Platform.Models;
using Freelance_Project_Management_Platform.Request;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Freelance_Project_Management_Platform.Services.Implementations
{
    public class ProposalService : IProposalService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        private readonly IUserLoggerService _logger;
        public ProposalService(DataContext context, IMapper mapper, ICurrentUserService currentUser, IUserLoggerService logger)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<ApiResponse<string>> CreateProposal(int projectId, AddProposal request)
        {
            try
            {
                var userId = _currentUser.UserId;

                var user = await _context.Users.FindAsync(userId);
                if (user == null || user.Role != USER_ROLE.FREELANCER)
                    return ApiResponseFactory.Forbidden<string>("Only freelancers can submit proposals");

                var alreadySubmitted = await _context.Proposals
                    .AnyAsync(p => p.ProjectId == projectId && p.FreelancerId == userId);
                if (alreadySubmitted)
                    return ApiResponseFactory.Conflict<string>("You have already submitted a proposal for this project");

                var projectExists = await _context.Projects.AnyAsync(p => p.Id == projectId);
                if (!projectExists)
                    return ApiResponseFactory.NotFound<string>("Project not found");

                var newProposal = _mapper.Map<Proposal>(request);
                newProposal.ProjectId = projectId;
                newProposal.FreelancerId = userId;
                newProposal.CreatedAt = DateTime.UtcNow;

                await _context.Proposals.AddAsync(newProposal);
                await _context.SaveChangesAsync();

                return ApiResponseFactory.Success("Proposal created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Unexpected error in {MethodName}", nameof(CreateProposal));
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred");
            }
        }

        public async Task<ApiResponse<string>> DeleteProposal(int proposalId)
        {
            try
            {
                var userId = _currentUser.UserId;
                var proposal = await _context.Proposals
                    .FirstOrDefaultAsync(p => p.Id == proposalId && p.FreelancerId == userId);

                if (proposal == null)
                {
                    return ApiResponseFactory.NotFound<string>("Proposal not found or access denied");
                }

                _context.Proposals.Remove(proposal);
                await _context.SaveChangesAsync();

                return ApiResponseFactory.Success("Proposal deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Unexpected error in {MethodName}", nameof(DeleteProposal));
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred");
            }
        }

        public async Task<ApiResponse<string>> UpdateProposal(int proposalId, AddProposal request)
        {
            try
            {
                var userId = _currentUser.UserId;
                var proposal = await _context.Proposals
                   .FirstOrDefaultAsync(p => p.Id == proposalId && p.FreelancerId == userId);

                if (proposal == null)
                {
                    return ApiResponseFactory.NotFound<string>("Proposal not found or access denied");
                }

                _mapper.Map(request, proposal);
                await _context.SaveChangesAsync();

                return ApiResponseFactory.Success("Proposal Updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Unexpected error in {MethodName}", nameof(UpdateProposal));
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred");
            }
        }

        public async Task<ApiResponse<PagedResult<ProposalDto>>> GetProjectProposals(int projectId, PaginationParams pagination)
        {
            try
            {
                var userId = _currentUser.UserId;

                if (pagination.Page <= 0) pagination.Page = 1;

                var query = _context.Proposals
                    .AsNoTracking()
                    .Where(p => p.ProjectId == projectId && p.Project.ClientId == userId);

                var totalCount = await query.CountAsync();

                var proposals = await query
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                var result = new PagedResult<ProposalDto>
                {
                    Items = _mapper.Map<List<ProposalDto>>(proposals),
                    TotalCount = totalCount,
                    Page = pagination.Page,
                    PageSize = pagination.PageSize
                };

                return ApiResponseFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Unexpected error in {MethodName}", nameof(GetProjectProposals));
                return ApiResponseFactory.ServerError<PagedResult<ProposalDto>>("Unexpected error occurred");
            }
        }

        public async Task<ApiResponse<string>> AcceptProposal(int proposalId)
        {
            try
            {
                var proposal = await _context.Proposals
                .Include(p => p.Project)
                .FirstOrDefaultAsync(p => p.Id == proposalId && p.Project.ClientId == _currentUser.UserId);

                if (proposal == null)
                    return ApiResponseFactory.NotFound<string>("Proposal not found or access denied");

                if (proposal.STATUS != PROPOSAL_STATUS.PENDING)
                    return ApiResponseFactory.BadRequest<string>("Proposal has already been accepted or rejected");

                proposal.STATUS = PROPOSAL_STATUS.ACCEPTED;
                proposal.Project.AcceptedFreelancerId = proposal.FreelancerId;
                proposal.Project.Status = PROJECT_STATUS.IN_PROGRESS;

                var otherProposals = await _context.Proposals
                    .Where(p => p.ProjectId == proposal.ProjectId && p.Id != proposalId)
                    .ToListAsync();
                otherProposals.ForEach(p => p.STATUS = PROPOSAL_STATUS.REJECTED);

                await _context.SaveChangesAsync();

                return ApiResponseFactory.Success("Proposal has been Accepted");
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Unexpected error in {MethodName}", nameof(AcceptProposal));
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred");
            }
        }

        public async Task<ApiResponse<string>> RejectProposal(int proposalId)
        {
            try
            {
                var proposal = await _context.Proposals
                .Include(p => p.Project)
                .FirstOrDefaultAsync(p => p.Id == proposalId && p.Project.ClientId == _currentUser.UserId);

                if (proposal == null)
                    return ApiResponseFactory.NotFound<string>("Proposal not found or access denied");

                if (proposal.STATUS != PROPOSAL_STATUS.PENDING)
                    return ApiResponseFactory.BadRequest<string>("Proposal has already been accepted or rejected");

                proposal.STATUS = PROPOSAL_STATUS.REJECTED;
                await _context.SaveChangesAsync();

                return ApiResponseFactory.Success("Proposal has been Rejected");
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Unexpected error in {MethodName}", nameof(RejectProposal));
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred");
            }
        }
    }
}
