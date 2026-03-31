namespace Freelance_Project_Management_Platform.Request
{
    public class AddTask
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
    }
}
