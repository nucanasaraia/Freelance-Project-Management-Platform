using Freelance_Project_Management_Platform.Enum;

namespace Freelance_Project_Management_Platform.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public TASK_STATUS Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int AssignedToUserId { get; set; }
        public User AssignedToUser { get; set; }
    }
}
