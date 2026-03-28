using AutoMapper;
using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.Data;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Enum;
using Freelance_Project_Management_Platform.Models;
using Freelance_Project_Management_Platform.Request;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Freelance_Project_Management_Platform.Services.Implementations
{
    public class ProposalService : IProposalService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        public ProposalService(DataContext context, IMapper mapper, ICurrentUserService currentUser)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<ApiResponse<string>> CreateProposal(int projectId, AddProposal request)
        {
            try
            {
                var userId = _currentUser.UserId;

                var newProposal = _mapper.Map<Proposal>(request);

                newProposal.ProjectId = projectId;
                newProposal.FreelancerId = userId;
                newProposal.CreatedAt = DateTime.UtcNow;

                await _context.Proposals.AddAsync(newProposal);
                await _context.SaveChangesAsync();

                return ApiResponseFactory.Success("Proposal created successfully");
            }
            catch
            {
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
            catch
            {
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
            catch
            {
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred");
            }
        }

        public async Task<ApiResponse<List<ProposalDto>>> GetProjectProposals(int projectId)
        {
            try
            {
                var userId = _currentUser.UserId;

                var proposals = await _context.Proposals
                    .Include(p => p.Project)
                    .Where(p => p.ProjectId == projectId && p.Project.ClientId == userId)
                    .ToListAsync();

                if (!proposals.Any())
                    return ApiResponseFactory.Success(new List<ProposalDto>());

                var result = _mapper.Map<List<ProposalDto>>(proposals);
                return ApiResponseFactory.Success(result);
            }
            catch
            {
                return ApiResponseFactory.ServerError<List<ProposalDto>>("Unexpected error occurred");
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
            catch
            {
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
            catch
            {
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred");
            }
        }
    }
}
