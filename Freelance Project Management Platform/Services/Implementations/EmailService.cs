using Freelance_Project_Management_Platform.Configurations;
using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Freelance_Project_Management_Platform.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtp;
        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtp = smtpSettings.Value;
        }

        public async Task<ApiResponse<string>> SendVerificationCode(string toEmail, string userName, string code)
        {
            try
            {
                var subject = "Your Verification Code";
                var body = GenerateVerificationEmailTemplate(userName, code);
                await SendEmailAsync(toEmail, subject, body);
                return ApiResponseFactory.Success("Verification code email sent.");
            }
            catch (SmtpException ex)
            {
                return ApiResponseFactory.ServerError<string>($"Failed to send verification email");
            }
            catch (Exception ex)
            {
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred while sending email");
            }
        }

        public async Task<ApiResponse<string>> SendResetPasswordLink(string toEmail, string userName, string resetLink)
        {
            try
            {
                var subject = "Reset Your Password";
                var body = GeneratePasswordResetEmailTemplate(userName, resetLink);
                await SendEmailAsync(toEmail, subject, body);
                return ApiResponseFactory.Success("Password reset email sent.");
            }
            catch (SmtpException ex)
            {
                return ApiResponseFactory.ServerError<string>("Failed to send password reset email");
            }
            catch (Exception ex)
            {
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred while sending email");
            }
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            using var mail = new MailMessage
            {
                From = new MailAddress(_smtp.SenderEmail, _smtp.SenderName),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            mail.To.Add(toEmail);

            using var smtpClient = new SmtpClient(_smtp.Host)
            {
                Port = _smtp.Port,
                EnableSsl = _smtp.EnableSsl,
                Credentials = new NetworkCredential(_smtp.SenderEmail, _smtp.AppPassword)
            };

            await smtpClient.SendMailAsync(mail);
        }
        private static string GeneratePasswordResetEmailTemplate(string userName, string resetLink) =>
           $@"
<h2>Password Reset</h2>
<p>Hello {userName},</p>
<p>You requested to reset your password.</p>
<p>
<a href='{resetLink}'>Click here to reset your password</a>
</p>
<p>This link will expire in 1 hour.</p>";

        private static string GenerateVerificationEmailTemplate(string userName, string code) =>
            $@"
<h2>Email Verification</h2>
<p>Hello {userName},</p>
<p>Your verification code is:</p>
<h1>{code}</h1>
<p>This code will expire shortly.</p>";
    }
}
