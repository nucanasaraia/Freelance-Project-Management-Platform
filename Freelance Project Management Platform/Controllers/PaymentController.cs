using Freelance_Project_Management_Platform.Request;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freelance_Project_Management_Platform.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("project/{projectId}")]
        public async Task<IActionResult> CreatePayment(int projectId, AddPayment request)
        {
            var result = await _paymentService.CreatePayment(projectId, request);
            return StatusCode((int)result.Status, result);
        }

        [HttpPatch("{id}/confirm")]
        public async Task<IActionResult> ConfirmPayment(int id)
        {
            var result = await _paymentService.ConfirmPayment(id);
            return StatusCode((int)result.Status, result);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetProjectPayments(int projectId)
        {
            var result = await _paymentService.GetProjectPayments(projectId);
            return StatusCode((int)result.Status, result);
        }
    }
}