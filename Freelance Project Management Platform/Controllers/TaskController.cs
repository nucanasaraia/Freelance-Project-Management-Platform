using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Enum;
using Freelance_Project_Management_Platform.Request;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Freelance_Project_Management_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost("add-task")]
        public async Task<IActionResult> CreateTask(int projectId, AddTask request)
        {
            var result = await _taskService.CreateTask(projectId, request);
            return StatusCode((int)result.Status, result);
        }

        [HttpGet("project-tasks")]
        public async Task<IActionResult> GetProjectTasks(int projectId)
        {
            var result =  await _taskService.GetProjectTasks(projectId);
            return StatusCode((int)result.Status,result);
        }

        [HttpPost("assign-task")]
        public async Task<IActionResult> AssignTask(int taskId, int assigneeId)
        {
            var result = await _taskService.AssignTask(taskId, assigneeId);
            return StatusCode((int)result.Status, result);
        }

        [HttpDelete("delete-task")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var result = await _taskService.DeleteTask(taskId);
            return StatusCode((int)result.Status, result);
        }

        [HttpPut("update-task-status")]
        public async Task<IActionResult> UpdateTaskStatus(int taskId, TASK_STATUS status)
        {
            var result = await _taskService.UpdateTaskStatus(taskId, status);
            return StatusCode((int)result.Status, result);
        }

    }
}
