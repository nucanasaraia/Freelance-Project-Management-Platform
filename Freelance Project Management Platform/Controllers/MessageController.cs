using Freelance_Project_Management_Platform.Request;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Freelance_Project_Management_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("send/{receiverId}")]
        public async Task<IActionResult> SendMessage(int receiverId, AddMessage request)
        {
            var result = await _messageService.SendMessage(receiverId, request);
            return StatusCode((int)result.Status, result);
        }

        [HttpGet("conversation/{otherUserId}")]
        public async Task<IActionResult> GetConversation(int otherUserId)
        {
            var result = await _messageService.GetConversation(otherUserId);
            return StatusCode((int)result.Status, result);
        }
    }
}