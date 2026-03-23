namespace Freelance_Project_Management_Platform.Request
{
    public class VerifyEmailRequest
    {
        public string Email { get; set; }
        public string VerificationCode { get; set; }
    }
}
