using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_recruitment_system.Models.Entities
{
    public class CompanyImages:BaseEntity
    {
        public string Image { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
