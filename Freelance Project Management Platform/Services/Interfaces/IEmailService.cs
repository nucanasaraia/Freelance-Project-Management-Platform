using Freelance_Project_Management_Platform.CORE;

namespace Freelance_Project_Management_Platform.Services.Interfaces
{
    public interface IEmailService
    {
        Task<ApiResponse<string>> SendResetPasswordLink(string toEmail, string userName, string resetLink);
        Task<ApiResponse<string>> SendVerificationCode(string toEmail, string userName, string code);
    }
}
