namespace Freelance_Project_Management_Platform.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int SenderId { get; set; }
        public string? SenderUsername { get; set; }
        public int ReceiverId { get; set; }
        public string? ReceiverUsername { get; set; }
    }
}
