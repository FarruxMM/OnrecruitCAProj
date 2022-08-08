using System;
using System.ComponentModel.DataAnnotations;

namespace Online_recruitment_system.Models.Entities
{
    public class Education : BaseEntity
    {
        public string University { get; set; }

        public string Degree { get; set; }
   
        public DateTime Years { get; set; }

        public string Faculty { get; set; }

        public int CandidateId { get; set; }

        public Candidate Candidate { get; set; }
    }
}
