using Freelance_Project_Management_Platform.Enum;

namespace Freelance_Project_Management_Platform.Models
{
    public class Proposal
    {
        public int Id { get; set; }
        public string? ProposalText { get; set; }
        public PROPOSAL_STATUS STATUS { get; set; } = PROPOSAL_STATUS.PENDING;
        public DateTime CreatedAt { get; set; }

        public int FreelancerId { get; set; }
        public User Freelancer { get; set; } = null!;

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    }
}
