using AutoMapper;
using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.Data;
using Freelance_Project_Management_Platform.Enum;
using Freelance_Project_Management_Platform.Models;
using Freelance_Project_Management_Platform.Request;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Net;
using System.Security.Cryptography;

namespace Freelance_Project_Management_Platform.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IEmailService _emailService;
        public AuthService(DataContext context, IMapper mapper, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
        }

        public Task<ApiResponse<string>> ForgotPassword(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<UserToken>> LogIn(LogInRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<UserToken>> RefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<string>> Registration(RegistrationRequest request)
        {
            var email = request.Email.ToLower().Trim();
            if (await _context.Users.AnyAsync(e => e.Email == email))
            {
                return ApiResponseFactory.Conflict<string>("Email already exists");
            }

            var user = new User
            {
                UserName = request.UserName,
                Email = email,
                PasswordHash = _passwordHasher.HashPassword(null, request.Password),
                Role = USER_ROLE.CLIENT,
                VerificationCode = GenerateVerificationCode(),
                VerificationCodeExpires = DateTime.UtcNow.AddMinutes(10)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var emailResult = await _emailService.SendVerificationCode(user.Email, user.UserName, user.VerificationCode);
            if(emailResult.Status != HttpStatusCode.OK)
            {
                return ApiResponseFactory.ServerError<string>("Failed to send verification email");
            }

            return ApiResponseFactory.Success("Registration successful. Verify email.");
        }

        public Task<ApiResponse<string>> ResetPassword(PasswordResetRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<string>> VerifyEmail(VerifyEmailRequest request)
        {
            var email = request.Email.Trim().ToLower();
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email);
            if (user == null)
            {
                return ApiResponseFactory.NotFound<string>("User not found");
            }
            if(user.VerificationAttempts >= 5)
            {
                return ApiResponseFactory.Conflict<string>("Too many attempts");
            }
            if (user.VerificationCodeExpires < DateTime.UtcNow)
            { 
                return ApiResponseFactory.Conflict<string>("Verification Code expired"); 
            }
            if(user.VerificationCode != request.VerificationCode)
            {
                user.VerificationAttempts++;
                await _context.SaveChangesAsync();
                return ApiResponseFactory.Conflict<string>("Verification failed. Code is incorrect");
            }

            user.EmailVerified = true;
            user.VerificationCode = null;
            user.VerificationAttempts = 0;
            user.VerificationCodeExpires = null;

            await _context.SaveChangesAsync();
            return ApiResponseFactory.Success("Email verified successfully");
        }

        private static string GenerateSecureToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        private static string GenerateVerificationCode() => RandomNumberGenerator.GetInt32(1000000, 9999999).ToString();
    }
}
