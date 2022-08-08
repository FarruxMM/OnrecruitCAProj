using Online_recruitment_system.Models.Entities;
using System.Collections.Generic;

namespace Online_recruitment_system.Models.ViewModels
{
    public class JobDetailsViewModel
    {
        public AppUser AppUser { get; set; }
        public Candidate Candidate { get; set; }
        public Application Application { get; set; }
        public Job Job { get; set; }
    }
}
