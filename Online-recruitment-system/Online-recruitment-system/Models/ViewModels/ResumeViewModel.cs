using Online_recruitment_system.Models.Entities;
using System.Collections.Generic;

namespace Online_recruitment_system.Models.ViewModels
{
    public class ResumeViewModel
    {
        public Candidate  Candidate { get; set; }
        public List<Resume> Resumes { get; set; }
    }
}
