namespace Freelance_Project_Management_Platform.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public required string TokenHash { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;
    }
}
