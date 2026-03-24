using Freelance_Project_Management_Platform.Enum;
using Freelance_Project_Management_Platform.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public USER_ROLE Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public bool EmailVerified { get; set; }

    // Email verification
    public string? VerificationCode { get; set; }
    public int VerificationAttempts { get; set; }
    public DateTime? VerificationCodeExpires { get; set; }

    // Password reset
    public string? PasswordResetTokenHash { get; set; }
    public DateTime? PasswordResetTokenExpires { get; set; }

    // Relationships
    public List<Project> ClientProjects { get; set; }
    public List<Project> AcceptedProjects { get; set; }

    public List<Proposal> Proposals { get; set; }
    public List<TaskItem> AssignedTasks { get; set; }

    public List<Message> SentMessages { get; set; }
    public List<Message> ReceivedMessages { get; set; }
}