using Online_recruitment_system.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_recruitment_system.Models.ViewModels
{
    public class ViewCandidateProfileViewModel
    {
        public Candidate Candidate { get; set; }
        public List<Candidate> Candidates { get; set; }
    }
}
