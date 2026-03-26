using Freelance_Project_Management_Platform.Enum;

namespace Freelance_Project_Management_Platform.DTOs
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public USER_ROLE Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }      
    }
}
