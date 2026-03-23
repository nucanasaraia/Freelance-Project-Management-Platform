namespace Freelance_Project_Management_Platform.Models
{
    public class UserToken
    {
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
