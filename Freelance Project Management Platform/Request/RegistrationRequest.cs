using Freelance_Project_Management_Platform.Enum;

namespace Freelance_Project_Management_Platform.Request
{
    public class RegistrationRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public USER_ROLE Role { get; set; } = USER_ROLE.CLIENT;
    }
}
