using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Online_recruitment_system.Models.ViewModels
{
    public class PostJobViewModel
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PositionName { get; set; }
        public int Category { get; set; }
        public int JobType { get; set; }
        public int Position { get; set; }
        public string Location { get; set; }
        public int Salary { get; set; } 
        public int Experience { get; set; } 
        public int JobSituation { get; set; } 
        public DateTime CreatedDate { get; set; }

        public SelectList JobTypes { get; set; }
        public SelectList Positions { get; set; }
        public SelectList Categories { get; set; }

    }
}
