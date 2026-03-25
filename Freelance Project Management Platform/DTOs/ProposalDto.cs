using Freelance_Project_Management_Platform.Enum;
using Freelance_Project_Management_Platform.Models;

namespace Freelance_Project_Management_Platform.DTOs
{
    public class ProposalDto
    {
        public int Id { get; set; }
        public string? ProposalText { get; set; }
        public PROPOSAL_STATUS STATUS { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
