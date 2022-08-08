using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Online_recruitment_system.Models.Entities
{
    public class Conference :BaseEntity
    {
        public string ConferenceName { get; set; }
        [MaxLength(4)]
        public string PinCode { get; set; }
        public Guid ConferenceId { get; set; }
    }
}
