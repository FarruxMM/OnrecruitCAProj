using Online_recruitment_system.Models.Entities;
using System.Collections.Generic;

namespace Online_recruitment_system.Models.ViewModels
{
    public class FilterCandidateViewModel
    {
        public List<string> Filter { get; set; }
        public List<Candidate> Candidates { get; set; }
        public PaginationModel PaginationModel { get; set; }

    }
}
