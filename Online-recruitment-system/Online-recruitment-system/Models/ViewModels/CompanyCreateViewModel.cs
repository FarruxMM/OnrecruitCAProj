using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Online_recruitment_system.Models.Entities;

namespace Online_recruitment_system.Models.ViewModels
{
    public class CompanyCreateViewModel
    {
        public AppUser AppUser { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AppUserId { get; set; }
        public int Location { get; set; }
        public int UserType { get; set; }
        public IFormFile Image { get; set; }

        public SelectList Locations { get; set; }
    }
}
