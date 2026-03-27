using Freelance_Project_Management_Platform.Models;

namespace Freelance_Project_Management_Platform.DTOs
{
    public class FreelancerDashboardDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public List<ProjectDto>? AcceptedProjects { get; set; }  
        public List<ProposalDto>? Proposals { get; set; }      
        public List<TaskDto>? AssignedTasks { get; set; }
    }
}
