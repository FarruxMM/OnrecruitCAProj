using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_recruitment_system.Models.Entities
{
    public class Company :BaseEntity
    {
        [Required,MaxLength(150)]
        public string Name { get; set; }

        [Required, MaxLength(500)]
        public string About { get; set; }

        [Required, MaxLength(200)]
        public string Email { get; set; }

        public string AppUserId { get; set; }

        public virtual AppUser Appuser { get; set; }

        [Required, MaxLength(200)]
        public string PhoneNumber { get; set; }

        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
        [Required]
        public int Usertype { get; set; }

        public List<Job> JobList { get; set; }

        public List<CompanyImages> CompanyImages { get; set; }

        [NotMapped]
        public List<IFormFile> CompanyImgs { get; set; }

        public bool IsActive { get; set; }

        public string Image { get; set; }
        [NotMapped]
        public IFormFile Img { get; set; }
    }
}
