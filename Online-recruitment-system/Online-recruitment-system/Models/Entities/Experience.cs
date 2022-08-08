using System.ComponentModel.DataAnnotations;

namespace Online_recruitment_system.Models.Entities
{
    public class Experience : BaseEntity
    {
        public int Year { get; set; }
        public string Location { get; set; }
        public string Position { get; set; }

        public int CandidateId { get; set; }
        public virtual Candidate Candidate { get; set; }

    }
}
