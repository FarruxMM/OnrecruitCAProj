using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Online_recruitment_system.Models.Entities
{
    public class Gender : BaseEntity
    {
        [Required,MaxLength(150)]
        public string Name { get; set; }
        public List<Candidate> Candidates { get; set; }
    }
}
