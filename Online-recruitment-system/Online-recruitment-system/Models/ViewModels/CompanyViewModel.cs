using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Online_recruitment_system.Models.Entities;
using System.Collections.Generic;

namespace Online_recruitment_system.Models.ViewModels
{
    public class CompanyViewModel
    {
        public PaginationModel Pagination { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Company Company { get; set; }
        public List<Company> Companies { get; set; }
        public SelectList Locations { get; set; }
        public string Image { get; set; }

        public IFormFile CompanyImg { get; set; }
        public List<IFormFile> CompanyImgs { get; set; }

    }
}
