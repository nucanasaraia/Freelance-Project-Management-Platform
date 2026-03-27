using Freelance_Project_Management_Platform.Enum;

namespace Freelance_Project_Management_Platform.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public PAYMENT_STATUS Status { get; set; } = PAYMENT_STATUS.PENDING;
        public DateTime CreatedAt { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int ClientId { get; set; }
        public User Client {  get; set; }

        public int FreelancerId { get; set; }
        public User Freelancer { get; set; }
    }
}
