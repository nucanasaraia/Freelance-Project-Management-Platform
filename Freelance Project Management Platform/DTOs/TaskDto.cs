using Freelance_Project_Management_Platform.Enum;
using Freelance_Project_Management_Platform.Models;

namespace Freelance_Project_Management_Platform.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public TASK_STATUS Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
