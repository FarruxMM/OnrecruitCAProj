using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Online_recruitment_system.Models.Entities
{
    public class Skill : BaseEntity
    {
        [Required,StringLength(150)]
        public string Name { get; set; }
        public List<SkillToCandidate> SkillToCandidates { get; set; }

    }
}
