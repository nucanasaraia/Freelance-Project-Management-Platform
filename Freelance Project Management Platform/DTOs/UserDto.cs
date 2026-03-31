using Freelance_Project_Management_Platform.Enum;

namespace Freelance_Project_Management_Platform.DTOs
{
    public class UserDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public USER_ROLE Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }      
    }
}
