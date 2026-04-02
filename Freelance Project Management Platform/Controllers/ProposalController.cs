using Freelance_Project_Management_Platform.Request;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freelance_Project_Management_Platform.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProposalController : ControllerBase
    {
        private readonly IProposalService _proposalService;

        public ProposalController(IProposalService proposalService)
        {
            _proposalService = proposalService;
        }

        [HttpPost("project/{projectId}")]
        public async Task<IActionResult> CreateProposal(int projectId, AddProposal request)
        {
            var result = await _proposalService.CreateProposal(projectId, request);
            return StatusCode((int)result.Status, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProposal(int id, AddProposal request)
        {
            var result = await _proposalService.UpdateProposal(id, request);
            return StatusCode((int)result.Status, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProposal(int id)
        {
            var result = await _proposalService.DeleteProposal(id);
            return StatusCode((int)result.Status, result);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetProjectProposals(int projectId)
        {
            var result = await _proposalService.GetProjectProposals(projectId);
            return StatusCode((int)result.Status, result);
        }

        [HttpPatch("{id}/accept")]
        public async Task<IActionResult> AcceptProposal(int id)
        {
            var result = await _proposalService.AcceptProposal(id);
            return StatusCode((int)result.Status, result);
        }

        [HttpPatch("{id}/reject")]
        public async Task<IActionResult> RejectProposal(int id)
        {
            var result = await _proposalService.RejectProposal(id);
            return StatusCode((int)result.Status, result);
        }
    }
}