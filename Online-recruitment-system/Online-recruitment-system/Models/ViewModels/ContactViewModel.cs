using Online_recruitment_system.Models.Entities;
using System.Collections.Generic;

namespace Online_recruitment_system.Models.ViewModels
{
    public class ContactViewModel
    {
        public Contact Contact { get; set; }
        public List<Branch> Branch { get; set; }
    }

}
