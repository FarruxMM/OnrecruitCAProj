using System.ComponentModel.DataAnnotations;

namespace Online_recruitment_system.Models.Entities
{
    public class SocialLink:BaseEntity
    {
        
        public string Name { get; set; }

        public int CandidateId { get; set; }

        public Candidate Candidate { get; set; }
    }

}
