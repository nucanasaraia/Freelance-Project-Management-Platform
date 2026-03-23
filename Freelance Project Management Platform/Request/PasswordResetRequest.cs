namespace Freelance_Project_Management_Platform.Request
{
    public class PasswordResetRequest
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
