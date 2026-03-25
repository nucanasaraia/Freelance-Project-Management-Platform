using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.Data;
using Freelance_Project_Management_Platform.Enum;
using Freelance_Project_Management_Platform.Models;
using Freelance_Project_Management_Platform.Request;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Cryptography;

public class AuthService : IAuthService
{
    private readonly DataContext _context;
    private readonly IEmailService _emailService;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(DataContext context, IEmailService emailService,ITokenService tokenService)
    {
        _context = context;
        _emailService = emailService;
        _passwordHasher = new PasswordHasher<User>();
        _tokenService = tokenService;
    }

    // REGISTER 
    public async Task<ApiResponse<string>> Registration(RegistrationRequest request)
    {
        try
        {
            var email = request.Email.Trim().ToLower();
            if (await _context.Users.AnyAsync(x => x.Email == email))
            {
                return Error<string>("Email already exists");
            }

            var user = new User
            {
                Username = request.Username,
                Email = email,
                PasswordHash = _passwordHasher.HashPassword(null, request.Password),
                Role = USER_ROLE.CLIENT,
                CreatedAt = DateTime.UtcNow,
                VerificationCode = GenerateVerificationCode(),
                VerificationCodeExpires = DateTime.UtcNow.AddMinutes(10)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var emailResult = await _emailService.SendVerificationCode(user.Email, user.Username, user.VerificationCode);

            if (emailResult.Status != HttpStatusCode.OK)
            {
                return Error<string>("Failed to send verification email");
            }

            return Success("Registration successful. Verify email.");
        }
        catch (Exception ex)
        {
            return Error<string>("An unexpected error occurred during registration");
        }
    }

    // VERIFY EMAIL 
    public async Task<ApiResponse<string>> VerifyEmail(VerifyEmailRequest request)
    {
        try
        {
            var email = request.Email.Trim().ToLower();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return Error<string>("Invalid request");
            }

            if (user.VerificationAttempts >= 5)
            {
                return Error<string>("Too many attempts");
            }

            if (user.VerificationCodeExpires < DateTime.UtcNow)
            {
                return Error<string>("Code expired");
            }

            if (user.VerificationCode != request.VerificationCode)
            {
                user.VerificationAttempts++;
                await _context.SaveChangesAsync();

                return Error<string>("Invalid code");
            }

            user.EmailVerified = true;
            user.VerificationCode = null;
            user.VerificationAttempts = 0;
            user.VerificationCodeExpires = null;

            await _context.SaveChangesAsync();

            return Success("Email verified successfully");

        }
        catch (Exception ex)
        {
            return Error<string>("An unexpected error occurred during email verification");
        }
    }

    // LOGIN 
    public async Task<ApiResponse<UserToken>> LogIn(LogInRequest request)
    {
        var username = request.Username.Trim().ToLower();
        User? user = null;

        try
        {
            user = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower() == username);

            if (user == null || !user.EmailVerified)
            {
                return Error<UserToken>("Invalid credentials");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result != PasswordVerificationResult.Success)
            {
                return Error<UserToken>("Invalid credentials");
            }

            var tokens = await GenerateTokens(user);
            await _context.SaveChangesAsync();

            return Success(tokens);
        }
            
        catch (Exception ex)
        {
            return Error<UserToken>("An unexpected error occurred during login");
        }
    }

    // REFRESH TOKEN 
    public async Task<ApiResponse<UserToken>> RefreshToken(string refreshToken)
    {
        try
        {
            var hash = _tokenService.HashToken(refreshToken);
            var token = await _context.RefreshTokens.Include(x => x.User)
                .FirstOrDefaultAsync(x => x.TokenHash == hash && !x.IsRevoked && x.ExpiresAt > DateTime.UtcNow);

            if (token == null)
            {
                return Error<UserToken>("Invalid refresh token");
            }

            token.IsRevoked = true;
            var tokens = await GenerateTokens(token.User);
            await _context.SaveChangesAsync();

            return Success(tokens);
        }
        catch (Exception ex)
        {
            return Error<UserToken>("An unexpected error occurred during token refresh");
        }
    }

    // FORGOT PASSWORD
    public async Task<ApiResponse<string>> ForgotPassword(string email)
    {
        try
        {
            email = email.Trim().ToLower();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return Success("If this email exists, a reset link was sent");
            }

            var rawToken = GenerateSecureToken();
            user.PasswordResetTokenHash = _tokenService.HashToken(rawToken);
            user.PasswordResetTokenExpires = DateTime.UtcNow.AddHours(1);

            var resetLink = $"https://yourfrontend.com/reset-password?token={rawToken}";
            await _emailService.SendResetPasswordLink(user.Email, user.Username, resetLink);
            await _context.SaveChangesAsync();

            return Success("If this email exists, a reset link was sent");
        }
        catch (Exception ex)
        {
            return Error<string>("An unexpected error occurred during password reset request");
        }
    }

    // RESET PASSWORD 
    public async Task<ApiResponse<string>> ResetPassword(PasswordResetRequest request)
    {
        try
        {
            var hash = _tokenService.HashToken(request.Token);
            var user = await _context.Users.FirstOrDefaultAsync(x =>
                x.PasswordResetTokenHash == hash &&
                x.PasswordResetTokenExpires > DateTime.UtcNow);

            if (user == null)
            {
                return Error<string>("Invalid or expired token");
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
            user.PasswordResetTokenHash = null;
            user.PasswordResetTokenExpires = null;

            await _context.SaveChangesAsync();

            return Success("Password reset successful");
        }
        catch (Exception ex)
        {
            return Error<string>("An unexpected error occurred during password reset");
        }
    }

    // HELPERS
    private async Task<UserToken> GenerateTokens(User user)
    {
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var hash = _tokenService.HashToken(refreshToken);

        _context.RefreshTokens.Add(new RefreshToken
        {
            TokenHash = hash,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });

        return new UserToken
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15)
        };
    }

    private static string GenerateSecureToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    private static string GenerateVerificationCode() => RandomNumberGenerator.GetInt32(100000, 999999).ToString();
    private static ApiResponse<T> Success<T>(T data) => ApiResponseFactory.Success(data);
    private static ApiResponse<T> Error<T>(string message) => ApiResponseFactory.Fail<T>(message, HttpStatusCode.InternalServerError);
}