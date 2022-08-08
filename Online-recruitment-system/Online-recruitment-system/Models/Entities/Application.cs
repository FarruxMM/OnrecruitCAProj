using System;

namespace Online_recruitment_system.Models.Entities
{
    public class Application : BaseEntity
    {
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; }

        public DateTime Date { get; set; }
    }
}
