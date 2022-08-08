using Microsoft.AspNetCore.Identity;

namespace Online_recruitment_system.Models.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public int UserType { get; set; }
        public Candidate Candidate { get; set; }
        public Company Company { get; set; }
        public bool IsActive { get;  set; }
    }
}
