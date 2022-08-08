namespace Online_recruitment_system.Models.Entities
{
    public class JobFollowers:BaseEntity
    {
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
       

    }
}
