using Freelance_Project_Management_Platform.Enum;

namespace Freelance_Project_Management_Platform.DTOs
{
    public class PaymentDto
    {
        public decimal Amount { get; set; }
        public PAYMENT_STATUS Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
