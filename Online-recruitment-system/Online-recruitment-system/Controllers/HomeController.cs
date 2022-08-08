using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_recruitment_system.DAL;
using Online_recruitment_system.Models.Entities;
using Online_recruitment_system.Models.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Online_recruitment_system.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext db;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(AppDbContext database, UserManager<AppUser> userManager)
        {
            this.db = database;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            Random rd = new Random();
            int random = rd.Next(1,6);

            HomeViewModel hvm = new HomeViewModel();
            hvm.Locations = db.Locations.Include(x => x.Companies).ThenInclude(x => x.JobList).ToList();
            hvm.Banner = await db.Banners.FirstOrDefaultAsync();
            hvm.Candidates = db.Candidates.Include(x => x.SkillToCandidates).ThenInclude(x => x.Skill)
             .Include(x => x.Profession).Take(6).ToList();
            hvm.Companies = db.Companies.ToList();
            hvm.JobSituations = db.JobSituations.ToList();
            hvm.Jobs = db.Jobs.Include(x => x.Category).Include(x => x.Company)
                .Include(x => x.JobSituation).Include(x => x.JobType).Include(x => x.Position).Skip(1).Take(4).ToList();
            hvm.Jobs2 = db.Jobs.Include(x => x.Category).Include(x => x.Company)
               .Include(x => x.JobSituation).Include(x => x.JobType).Include(x => x.Position).Take(8).ToList();
            hvm.Categoriess = new SelectList(db.Categories.ToList(), "Id", "Name");
            hvm.RandomNum = random;

            if (User.Identity.IsAuthenticated)
            {
                AppUser loggedUser = await _userManager.FindByNameAsync(User.Identity.Name);
                if (loggedUser == null)
                {
                    hvm.User = null;

                }
                hvm.Candidate = await db.Candidates.FirstOrDefaultAsync(x => x.AppUserId == loggedUser.Id);
                hvm.Company = await db.Companies.FirstOrDefaultAsync(x => x.AppUserId == loggedUser.Id);


                if (hvm.Candidate == null)
                {
                    hvm.UserType = 2;
                    ViewBag.CheckUser = 2;
                }
                if (hvm.Company == null)
                {
                    hvm.UserType = 1;
                    ViewBag.CheckUser = 1;
                }

                if (hvm.Company == null && hvm.Candidate == null)
                {
                    hvm.UserType = loggedUser.UserType;
                    ViewBag.CheckUser = loggedUser.UserType;
                }

                hvm.User = loggedUser;
            }

            TempData["Notification"] = "Saytimiza xos gelmisiniz";

            return View(hvm);
        }

    }
}
