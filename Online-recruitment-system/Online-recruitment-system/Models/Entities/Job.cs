using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Online_recruitment_system.Models.Entities
{
    public class Job : BaseEntity
    {
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int PositionId { get; set; }

        public virtual Position Position { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [Required, MaxLength(200)]
        public string Location { get; set; }

        [Required, MaxLength(200)]
        public string PositionName { get; set; }

        public int JobTypeId { get; set; }

        public virtual JobType JobType { get; set; }

        public int JobSituationId { get; set; }
        public JobSituation JobSituation { get; set; }

        //public List<JobFollowers> JobFollowers { get; set; }
        public List<Application> Applications { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        public int Salary { get; set; }

        public int Experience { get; set; }

        public string Image { get; set; }
        public bool IsActive { get; set; }


    }
}
