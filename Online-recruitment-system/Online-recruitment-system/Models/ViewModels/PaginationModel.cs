using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_recruitment_system.Models.ViewModels
{
    public class PaginationModel
    {
        public int TotalPage { get { return (int)Math.Ceiling((decimal)TotalItems / (decimal)ItemsPerPage); }  }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
    }
 
}
