namespace Freelance_Project_Management_Platform.Configurations
{
    public class SmtpSettings
    {
        public required string Host { get; set; }
        public int Port { get; set; }
        public required string SenderEmail { get; set; }
        public required string SenderName { get; set; }
        public required string AppPassword { get; set; }
        public bool EnableSsl { get; set; }
    }
}
