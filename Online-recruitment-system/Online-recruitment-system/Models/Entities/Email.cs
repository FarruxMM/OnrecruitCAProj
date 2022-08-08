using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Online_recruitment_system.Models.Entities
{
    public class Email:BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
