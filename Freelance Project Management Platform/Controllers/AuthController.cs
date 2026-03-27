using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.Models;
using Freelance_Project_Management_Platform.Request;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Freelance_Project_Management_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.Registration(request);
            return StatusCode((int)result.Status, result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn(LogInRequest request)
        {
            var response = await _authService.LogIn(request);

            if (response.Status != HttpStatusCode.OK)
                return StatusCode((int)response.Status, response);

            Response.Cookies.Append("refreshToken", response.Data.RefreshToken, new CookieOptions
            {
                HttpOnly = true,     
                Secure = true,      
                SameSite = SameSiteMode.Strict, 
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(response);
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailRequest request)
        {
            var result = await _authService.VerifyEmail(request);
            return StatusCode((int)result.Status, result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized("No refresh token found");

            var response = await _authService.RefreshToken(refreshToken);

            if (response.Status != HttpStatusCode.OK)
                return StatusCode((int)response.Status, response);

            Response.Cookies.Append("refreshToken", response.Data.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(response);
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var result = await _authService.ForgotPassword(email);
            return StatusCode((int)result.Status, result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(PasswordResetRequest request)
        {
            var result = await _authService.ResetPassword(request);
            return StatusCode((int)result.Status, result);
        }
    }
}
