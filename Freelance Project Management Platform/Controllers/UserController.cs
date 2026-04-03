using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Request;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Freelance_Project_Management_Platform.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllUsers([FromQuery] PaginationParams pagination)
        {
            var result = await _userService.GetAllUsers(pagination);
            return StatusCode((int)result.Status, result);
        }

        [HttpPost("ban-user/{id}")]
        public async Task<IActionResult> BanUser(int id)
        {
            var result = await _userService.BanUser(id);
            return StatusCode((int)result.Status, result);
        }
    }
}
