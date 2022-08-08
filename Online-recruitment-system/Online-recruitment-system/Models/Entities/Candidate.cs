using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_recruitment_system.Models.Entities
{
    public class Candidate : BaseEntity
    {
        [Required, MaxLength(150)]
        public string FirstName { get; set; }

        [Required, MaxLength(150)]
        public string LastName { get; set; }

        public string AppUserId { get; set; }

        public virtual AppUser Appuser { get; set; }

        public int Usertype { get; set; }


        [Required]
        public DateTime BirthDate { get; set; }

        public int GenderId { get; set; }

        public virtual Gender Gender { get; set; }

        [Required, MaxLength(150), EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(100)]
        public string PhoneNumber { get; set; }

        [MaxLength(750)]
        public string About { get; set; }

        public int ProfessionId { get; set; }

        public virtual Profession Profession { get; set; }

        public List<Experience> Experiences { get; set; }
        public List<SkillToCandidate> SkillToCandidates { get; set; }

        public List<Education> Educations { get; set; }
        public List<JobFollowers> JobFollowers { get; set; }
        public List<Application> Applications { get; set; }
        public List<SocialLink> SocialLinks { get; set; }
        public List<Resume> Resumes { get; set; }
        public bool IsActive { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Img { get; set; }
    }
}
