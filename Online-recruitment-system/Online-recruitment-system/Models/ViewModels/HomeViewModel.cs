using Microsoft.AspNetCore.Mvc.Rendering;
using Online_recruitment_system.Models.Entities;
using System.Collections.Generic;

namespace Online_recruitment_system.Models.ViewModels
{
    public class HomeViewModel
    {
        public int RandomNum { get; set; }
        public Company Company { get; set; }
        public Candidate Candidate { get; set; }
        public Banner Banner { get; set; }
        public List<Job> Jobs { get; set; }
        public List<Job> Jobs2 { get; set; }
        public List<JobSituation> JobSituations { get; set; }
        public List<Location> Locations { get; set; }
        public int Category { get; set; }
        public List<Company> Companies { get; set; }
        public List<Candidate> Candidates { get; set; }
        public int UserType { get; set; }
        public SelectList Categoriess { get; set; }
        public AppUser User { get; set; }

    }
}
