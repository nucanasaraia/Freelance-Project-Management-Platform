using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freelance_Project_Management_Platform.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetClientDashboard(int clientId)
        {
            var result = await _dashboardService.GetClientDashboard(clientId);
            return StatusCode((int)result.Status, result);
        }

        [HttpGet("freelancer/{freelancerId}")]
        public async Task<IActionResult> GetFreelancerDashboard(int freelancerId)
        {
            var result = await _dashboardService.GetFreelancerDashboard(freelancerId);
            return StatusCode((int)result.Status, result);
        }
    }
}