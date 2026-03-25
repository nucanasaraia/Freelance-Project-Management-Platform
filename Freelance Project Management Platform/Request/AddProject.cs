using Freelance_Project_Management_Platform.Enum;

namespace Freelance_Project_Management_Platform.Request
{
    public class AddProject
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal Budget { get; set; }
        public DateTime Deadline { get; set; }
    }
}
