using System.Collections.Generic;

namespace Online_recruitment_system.Models.Entities
{
    public class JobSituation:BaseEntity
    {
        public string Name { get; set; }

        public List<Job> Jobs { get; set; }

    }
}
