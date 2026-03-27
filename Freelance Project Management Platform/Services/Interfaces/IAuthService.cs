using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Models;
using Freelance_Project_Management_Platform.Request;
using Microsoft.AspNetCore.Identity.Data;

namespace Freelance_Project_Management_Platform.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> Registration(RegistrationRequest request);
        Task<ApiResponse<string>> VerifyEmail(VerifyEmailRequest request);
        Task<ApiResponse<AuthResponseDto>> LogIn(LogInRequest request);
        Task<ApiResponse<AuthResponseDto>> RefreshToken(string refreshToken);
        Task<ApiResponse<string>> ForgotPassword(string email);
        Task<ApiResponse<string>> ResetPassword(PasswordResetRequest request);
    }
}
