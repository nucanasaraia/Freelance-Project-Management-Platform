using Freelance_Project_Management_Platform.Enum;

namespace Freelance_Project_Management_Platform.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal Budget { get; set; }
        public DateTime Deadline { get; set; }
        public PROJECT_STATUS STATUS { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; }


        public int ClientId { get; set; }
        public User Client { get; set; }

        public int? AcceptedFreelancerId { get; set; }
        public User? AcceptedFreelancer { get; set; }

        public List<TaskItem> TaskItems { get; set; }
        public List<Proposal> Proposals { get; set; }
    }
}
