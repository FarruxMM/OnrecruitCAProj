using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_recruitment_system.DAL;
using Online_recruitment_system.Models.Entities;
using Online_recruitment_system.Models.ViewModels;
using Online_recruitment_system.Utils;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Online_recruitment_system.Controllers
{
    public class CompanyController : Controller
    {
        private readonly AppDbContext db;
        private readonly UserManager<AppUser> userManager;
        private readonly IWebHostEnvironment env;


        public CompanyController(AppDbContext database, UserManager<AppUser> _userManager, IWebHostEnvironment _env)
        {
            this.db = database;
            this.userManager = _userManager;
            this.env = _env;
        }

        public async Task<IActionResult> Index()
        {
            AppUser loggedUser = await userManager.FindByNameAsync(User.Identity.Name);
            Company company = await db.Companies.Include(x => x.Location).Include(x => x.CompanyImages).FirstOrDefaultAsync(x => x.AppUserId == loggedUser.Id);
            CompanyViewModel companyViewModel = new CompanyViewModel()
            {
                Company = company
            };

            return View(companyViewModel);
        }

        public async Task<IActionResult> CreateProfile()
        {
            CompanyCreateViewModel ccvm = new CompanyCreateViewModel();

            ccvm.Locations = new SelectList(db.Locations.ToList(), "Id", "Address");

            AppUser loggedUser = await userManager.FindByNameAsync(User.Identity.Name);

            if (loggedUser != null)
            {
                ccvm.AppUser = loggedUser;
                return View(ccvm);
            }

            return NoContent();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProfile(CompanyCreateViewModel ccvm)
        {

            Company company = new Company()
            {
                About = ccvm.About,
                AppUserId = ccvm.AppUserId,
                Email = ccvm.Email,
                Img = ccvm.Image,
                LocationId = ccvm.Location,
                Name = ccvm.Name,
                PhoneNumber = ccvm.PhoneNumber,
                Usertype = ccvm.UserType
            };

            if (company != null)
            {
                if (company.Img != null)
                {
                    if (!company.Img.IsImage())
                    {
                        ModelState.AddModelError("Img", "File Shekil olmalidir");
                        return View();
                    }

                    if (!company.Img.IsValidImage(700))
                    {
                        ModelState.AddModelError("Img", "Fayl max 700 kb ola biler!");
                        return View();
                    }

                    company.Image = await company.Img.UpLoad(env.WebRootPath, @"img/companyimg");
                }


                if (!ModelState.IsValid) return View(company);

                await db.Companies.AddAsync(company);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Company");

            }

            return NoContent();

        }

        public async Task<IActionResult> EditProfile(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }


            AppUser loggedUser = await userManager.FindByNameAsync(User.Identity.Name);
            Company company = await db.Companies.Include(x => x.CompanyImages).Include(x => x.Location).FirstOrDefaultAsync(x => x.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            CompanyViewModel cvm = new CompanyViewModel();
            cvm.AppUser = loggedUser;
            cvm.Company = company;
            cvm.AppUserId = loggedUser.Id;
            cvm.Image = company.Image;

            ViewBag.Locations = new SelectList(db.Locations.ToList(), "Id", "Address");
            return View(cvm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(int id, CompanyViewModel companyvm)
        {
            if (id != companyvm.Company.Id)
            {
                return NotFound();
            }

            if (companyvm.CompanyImg != null)
            {
                if (!companyvm.CompanyImg.IsImage())
                {
                    ModelState.AddModelError("Img", "File Shekil olmalidir");
                    return View();
                }

                if (!companyvm.CompanyImg.IsValidImage(700))
                {
                    ModelState.AddModelError("Img", "Fayl max 700 kb ola biler!");
                    return View();
                }

                string filepath = Path.Combine(env.WebRootPath, @"img\companyimg", companyvm.Company.Image);
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }

                companyvm.Company.Image = await companyvm.CompanyImg.UpLoad(env.WebRootPath, @"img/companyimg");

            }

            if (!(companyvm.CompanyImgs == null || companyvm.CompanyImgs.Count() == 0))
            {
                foreach (var item in companyvm.CompanyImgs)
                {
                    CompanyImages companyImages = new CompanyImages();
                    companyImages.CompanyId = companyvm.Company.Id;
                    companyImages.Image = await item.UpLoad(env.WebRootPath, @"img/companyimages");
                    db.CompanyImages.Add(companyImages);
                    await db.SaveChangesAsync();
                }
            }

            if (ModelState.IsValid)
            {
                db.Companies.Update(companyvm.Company);
                await db.SaveChangesAsync();

                return RedirectToAction("Index", "Company");
            }

            if (!ModelState.IsValid) return View(companyvm.Company);

            return View();

        }

        public async Task<IActionResult> Posted()
        {

            AppUser loggedUser = await userManager.FindByNameAsync(User.Identity.Name);
            Company company = await db.Companies.Include(x => x.CompanyImages).Include(x => x.Location)
                .Include(x => x.JobList).ThenInclude(x => x.Category).Include(x => x.JobList).ThenInclude(x => x.JobType)
                .FirstOrDefaultAsync(x => x.AppUserId == loggedUser.Id);

            if (company.JobList.Count()==0)
            {
                TempData["null"] = "You don't have posted jobs!";
            }

            return View(company);
        }


        public IActionResult PostJob()
        {
            PostJobViewModel pjvm = new PostJobViewModel();

            pjvm.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            pjvm.Positions = new SelectList(db.Positions.ToList(), "Id", "Name");
            pjvm.JobTypes = new SelectList(db.JobTypes.ToList(), "Id", "Type");
            return View(pjvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostJob(PostJobViewModel pjvm)
        {
            AppUser loggedUser = await userManager.FindByNameAsync(User.Identity.Name);
            Company company = await db.Companies.Include(x => x.CompanyImages).FirstOrDefaultAsync(x => x.AppUserId == loggedUser.Id);

            if (pjvm.Position==0 )
            {
                pjvm.Position = 1;
            }
            if (pjvm.JobType == 0)
            {
                pjvm.JobType = 1;
            }
            if (pjvm.Category == 0)
            {
                pjvm.Category = 1;
            }

            Job job = new Job();
            job.CategoryId = pjvm.Category;
            job.PositionId = pjvm.Position;
            job.JobTypeId = pjvm.JobType;
            job.Salary = pjvm.Salary;
            job.Experience = pjvm.Experience;
            job.PositionName = pjvm.PositionName;
            job.Location = pjvm.Location;
            job.CreateDate = pjvm.CreatedDate;
            job.JobSituationId = pjvm.JobSituation;
            job.CompanyId = company.Id;
            job.Image = company.Image;
            job.IsActive = true;

            if (!ModelState.IsValid) return View();
            await db.Jobs.AddAsync(job);
            await db.SaveChangesAsync();
            return RedirectToAction("SendMessageToAll", "Jobs");
        }

    }
}
