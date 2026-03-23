namespace Freelance_Project_Management_Platform.Configurations
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string AppPassword { get; set; }
        public bool EnableSsl { get; set; }
    }
}
