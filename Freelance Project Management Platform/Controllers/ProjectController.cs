using Freelance_Project_Management_Platform.Request;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Freelance_Project_Management_Platform.Controllers
{
    [EnableRateLimiting("general")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(AddProject request)
        {
            var result = await _projectService.CreateProject(request);
            return StatusCode((int)result.Status, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var result = await _projectService.GetAllProjects();
            return StatusCode((int)result.Status, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var result = await _projectService.GetProject(id);
            return StatusCode((int)result.Status, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, AddProject request)
        {
            var result = await _projectService.UpdateProject(id, request);
            return StatusCode((int)result.Status, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var result = await _projectService.DeleteProject(id);
            return StatusCode((int)result.Status, result);
        }

        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> MarkAsCompleted(int id)
        {
            var result = await _projectService.MarkAsCompleted(id);
            return StatusCode((int)result.Status, result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPatch("{id}/paid")]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var result = await _projectService.MarkAsPaid(id);
            return StatusCode((int)result.Status, result);
        }
    }
}