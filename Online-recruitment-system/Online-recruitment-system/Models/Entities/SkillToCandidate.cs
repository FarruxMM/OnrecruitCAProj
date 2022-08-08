namespace Online_recruitment_system.Models.Entities
{
    public class SkillToCandidate:BaseEntity
    {
        public int SkillId { get; set; }
        public Skill Skill { get; set; }
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
    }
}
