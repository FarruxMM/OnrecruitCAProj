using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Online_recruitment_system.Models.Entities;
using System;
using System.Collections.Generic;

namespace Online_recruitment_system.Models.ViewModels
{
    public class CandidateViewModel
    {
        public PaginationModel Pagination { get; set; }
        public Candidate Candidate { get; set; }
        public List<Candidate> Candidates { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string About { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public int ProfessionId { get; set; }
        public int GenderId { get; set; }

        public List<int> Skill { get; set; }
        public Education Education { get; set; }
        public Experience Experience { get; set; }
        public List<SocialLink> SocialLinks { get; set; }

        public SelectList SkillList { get; set; }
        public SelectList Genders { get; set; }
        public SelectList Professions { get; set; }

        public string Image { get; set; }
        public IFormFile Img { get; set; }

    }
}
