using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Online_recruitment_system.Models.Entities
{
    public class Position :BaseEntity
    {
        [Required,MaxLength(250)]
        public string Name { get; set; }
        public List<Job> Jobs { get; set; }
    }
}
