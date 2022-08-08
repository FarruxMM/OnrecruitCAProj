using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Online_recruitment_system.Models.Entities
{
    public class Branch:BaseEntity
    {
        [Required, MaxLength(150)]
        public string Name { get; set; }
        [Required, MaxLength(250)]
        public string Location { get; set; }
        [Required, MaxLength(150)]
        public string PhoneNumber { get; set; }
        [Required, MaxLength(150)]
        public string Email { get; set; }

    }
}
