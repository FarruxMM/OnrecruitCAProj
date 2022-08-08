using System.Collections.Generic;

namespace Online_recruitment_system.Models.Entities
{
    public class JobType : BaseEntity
    {
        public string Type { get; set; }
        public List<Job> Jobs { get; set; }
    }
}
