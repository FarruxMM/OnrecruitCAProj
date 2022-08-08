using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_recruitment_system.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_recruitment_system.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext db;

        public DashboardController(AppDbContext database)
        {
            this.db = database;
        }
        public IActionResult Index()
        {
            ViewBag.TotalBranches = db.Branches.Count();
            ViewBag.TotalCategories = db.Categories.Count();
            ViewBag.TotalLocations = db.Locations.Count();
            ViewBag.TotalJobs = db.Jobs.Count();
            ViewBag.TotalContact = db.Contacts.Count();
            ViewBag.TotalCandidates = db.Candidates.Count();
            ViewBag.TotalCompanies = db.Companies.Count();
            ViewBag.TotalEmails = db.Emails.Count();
            
            return View();
        }
    }
}
