using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Request;

namespace Freelance_Project_Management_Platform.Services.Interfaces
{
    public interface IProposalService
    {
        Task<ApiResponse<string>> CreateProposal(int projectId, AddProposal request);
        Task<ApiResponse<string>> UpdateProposal(int id, AddProposal request);
        Task<ApiResponse<string>> DeleteProposal(int id);
        Task<ApiResponse<List<ProposalDto>>> GetProjectProposals(int projectId);
        Task<ApiResponse<string>> AcceptProposal(int proposalId);
        Task<ApiResponse<string>> RejectProposal(int proposalId);
    }
}

