namespace Freelance_Project_Management_Platform.Request
{
    public class PasswordResetRequest
    {
        public required string Token { get; set; }
        public required string NewPassword { get; set; }
    }
}
