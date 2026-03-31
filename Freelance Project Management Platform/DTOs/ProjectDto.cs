using Freelance_Project_Management_Platform.Enum;

namespace Freelance_Project_Management_Platform.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }              
        public required string Title { get; set; }
        public string? Description { get; set; }
        public decimal Budget { get; set; }
        public DateTime Deadline { get; set; }

        public PROJECT_STATUS Status { get; set; }  
        public bool IsPaid { get; set; }          
        public DateTime CreatedAt { get; set; }
    }
}
