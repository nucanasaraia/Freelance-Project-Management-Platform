using Freelance_Project_Management_Platform.Enum;

namespace Freelance_Project_Management_Platform.Models
{
    public class Project
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public decimal Budget { get; set; }
        public DateTime Deadline { get; set; }
        public PROJECT_STATUS Status { get; set; } = PROJECT_STATUS.OPEN;
        public bool IsPaid { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public int ClientId { get; set; }
        public User Client { get; set; } = null!;

        public int? AcceptedFreelancerId { get; set; }
        public User? AcceptedFreelancer { get; set; }

        public List<TaskItem> TaskItems { get; set; } = new();
        public List<Proposal> Proposals { get; set; } = new();
    }
}
