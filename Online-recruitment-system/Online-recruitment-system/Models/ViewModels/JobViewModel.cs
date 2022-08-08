using Microsoft.AspNetCore.Mvc.Rendering;
using Online_recruitment_system.Models.Entities;
using System.Collections.Generic;

namespace Online_recruitment_system.Models.ViewModels
{
    public class JobViewModel
    {
        public int? UserType { get; set; }
        public List<Job> Jobs { get; set; }

        public PaginationModel Pagination { get; set; }

        public SelectList Categories { get; set; }

        public List<Category> Categoriesss { get; set; }
        public List<Location> Locations { get; set; }

        public string Category { get; set; }
        public string Location { get; set; }
        public Candidate Candidate { get; set; }

    }
}
