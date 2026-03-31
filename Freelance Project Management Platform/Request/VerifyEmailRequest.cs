namespace Freelance_Project_Management_Platform.Request
{
    public class VerifyEmailRequest
    {
        public required string Email { get; set; }
        public required string VerificationCode { get; set; }
    }
}
