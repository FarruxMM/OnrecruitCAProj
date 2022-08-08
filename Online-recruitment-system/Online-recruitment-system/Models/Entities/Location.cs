using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_recruitment_system.Models.Entities
{
    public class Location :BaseEntity
    {
        [Required,MaxLength(150)]
        public string Address { get; set; }

        //public List<Job> Jobs { get; set; }
        public List<Company> Companies { get; set; }

        public string Image { get; set; }
        [NotMapped]
        public IFormFile Img { get; set; }
    }
}
