using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_recruitment_system.DAL;
using Online_recruitment_system.Models.Entities;
using Online_recruitment_system.Models.Interface;
using Online_recruitment_system.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_recruitment_system.Controllers
{
    public class JobsController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly AppDbContext db;
        private readonly static int _itemsPerPage = 5;
        private readonly IEmailSender emailSender;

        public JobsController(AppDbContext database, IEmailSender emailSender, UserManager<AppUser> userManager)
        {
            this.db = database;
            this.emailSender = emailSender;
            this.userManager = userManager;
        }


        public async Task<IActionResult> Index(string name, int page = 1)
        {

            JobViewModel jvm = new JobViewModel();

            if (!string.IsNullOrEmpty(name))
            {
                var data = await db.Jobs.Where(x => x.Category.Name == name).Include(x => x.Company).Include(x => x.Category).Include(x => x.JobType)
             .Include(x => x.Position).ToListAsync();
                jvm.Jobs = data;
                jvm.Categoriesss = db.Categories.Include(x => x.Jobs).ToList();
                jvm.Locations = db.Locations.Include(x => x.Companies).ToList();
                jvm.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                var data2 = data.Count();
                PaginationModel pgmodel1 = new PaginationModel()
                {
                    TotalItems = data2,
                    ItemsPerPage = _itemsPerPage,
                    CurrentPage = page

                };

                jvm.Pagination = pgmodel1;
                return View(jvm);
            }
            else
            {
                var data = await db.Jobs.Include(x => x.Company).Include(x => x.Category).Include(x => x.JobType)
               .Include(x => x.Position).OrderByDescending(x => x.Id).Skip((page - 1) * _itemsPerPage)
               .Take(_itemsPerPage).ToListAsync();
                jvm.Jobs = data;
            }

            var data1 = db.Jobs.Count();

            if (User.Identity.IsAuthenticated)
            {
                AppUser loggedUser = await userManager.FindByNameAsync(User.Identity.Name);
                Candidate candidate = await db.Candidates.FirstOrDefaultAsync(x => x.AppUserId == loggedUser.Id);

                if (candidate == null)
                {
                    jvm.UserType = 2;
                }
                else
                {
                    jvm.UserType = 1;
                }

            }
            jvm.Categoriesss = db.Categories.Include(x => x.Jobs).ToList();
            jvm.Locations = db.Locations.Include(x => x.Companies).ToList();
            jvm.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");

            PaginationModel pgmodel = new PaginationModel()
            {
                TotalItems = data1,
                ItemsPerPage = _itemsPerPage,
                CurrentPage = page

            };

            jvm.Pagination = pgmodel;

            return View(jvm);
        }

        public async Task<IActionResult> AjaxIndex(string name)
        {
            JobViewModel jvm = new JobViewModel();

            if (!string.IsNullOrEmpty(name))
            {
                var data = await db.Jobs.Where(x => x.Category.Name == name).Include(x => x.Company).Include(x => x.Category).Include(x => x.JobType)
             .Include(x => x.Position).ToListAsync();
                jvm.Jobs = data;
            }

            var data1 = db.Jobs.Count();

            if (User.Identity.IsAuthenticated)
            {
                AppUser loggedUser = await userManager.FindByNameAsync(User.Identity.Name);
                Candidate candidate = await db.Candidates.FirstOrDefaultAsync(x => x.AppUserId == loggedUser.Id);

                if (candidate == null)
                {
                    jvm.UserType = 2;
                }
                else
                {
                    jvm.UserType = 1;
                }

            }
            jvm.Categoriesss = db.Categories.Include(x => x.Jobs).ToList();
            jvm.Locations = db.Locations.Include(x => x.Companies).ToList();
            jvm.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");

            int page = 1;

            PaginationModel pgmodel = new PaginationModel()
            {
                TotalItems = data1,
                ItemsPerPage = _itemsPerPage,
                CurrentPage = page

            };

            jvm.Pagination = pgmodel;

            return View(jvm);
        }

        public async Task<IActionResult> JobDetails(int? id, bool check)
        {
            JobDetailsViewModel jdvm = new JobDetailsViewModel();
            Job job = await db.Jobs.Include(x => x.Category).Include(x => x.Company).FirstOrDefaultAsync(x => x.Id == id);
            jdvm.Job = job;

            if (check)
            {
                AppUser loggedUser = await userManager.FindByNameAsync(User.Identity.Name);
                Candidate candidate = await db.Candidates.FirstOrDefaultAsync(x => x.AppUserId == loggedUser.Id);
                jdvm.Candidate = candidate;
                jdvm.AppUser = loggedUser;
            }

            else
            {
                jdvm.AppUser = null;
                jdvm.Candidate = null;
            }

            return View(jdvm);
        }

        public async Task<IActionResult> Appliers(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Job job = await db.Jobs.Include(x => x.Category).Include(x => x.JobType).Include(x => x.JobSituation).Include(x => x.Company)
                .Include(x => x.Position).FirstOrDefaultAsync(x => x.Id == id);
            List<Application> applications = await db.Applications.Include(x => x.Candidate).Include(x => x.Job).Where(x => x.JobId == job.Id).ToListAsync();

            if (applications == null)
            {
                return RedirectToAction("NoApplication", "Jobs");
            }

            List<Candidate> candidates = new List<Candidate>();

            foreach (var item in applications)
            {
                candidates = await db.Candidates.Where(x => x.Id == item.CandidateId).Include(x => x.Educations).Include(x => x.Experiences)
                    .Include(x => x.Profession).Include(x => x.SocialLinks).Include(x => x.SkillToCandidates).ThenInclude(x => x.Skill).ToListAsync();
            }

            if (candidates.Count() == 0)
            {
                TempData["NoCandidate"] = "There are no Candidate Who Applied this job";
            }

            ApplicationViewModel apvm = new ApplicationViewModel();
            apvm.Candidates = candidates;
            apvm.Job = job;

            return View(apvm);
        }

        public IActionResult NoApplication()
        {
            return View();
        }

        public IActionResult NoValidEmail()
        {
            return View();
        }

        public async Task<IActionResult> ApplyJob(int? id, int? job)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            if (job == null || job == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            Application application = new Application();
            application.Date = DateTime.Now;
            application.CandidateId = (int)id;
            application.JobId = (int)job;

            await db.Applications.AddAsync(application);
            db.SaveChanges();

            return View();
        }

        public IActionResult StopApply()
        {

            return View();
        }


        public async Task<JsonResult> FilterCategory(string name)
        {
            // code

            var data = await db.Jobs.Where(x => x.Category.Name == name).Include(x => x.Company).Include(x => x.Category).Include(x => x.JobType)
             .Include(x => x.Position).ToListAsync();

            return Json(new
            {
                status = 200,
                data = data
            });
        }


        public IActionResult SendMessageToAll()
        {

            List<Email> emails = new List<Email>();
            emails = db.Emails.ToList();
            foreach (var item in emails)
            {
                emailSender.SendEmailAsync(item.Name, "online recruitment system greetings", "There is New Job for u on our site");

            }
            return RedirectToAction("Posted", "Company");

        }

        public IActionResult SendMessage(string email)
        {
            List<Email> emails = db.Emails.ToList();

            foreach (var item in emails)
            {
                if (item.Name.Contains(email))
                {
                    return RedirectToAction("NoValidEmail" , "Jobs");
                }
            }

            if (!string.IsNullOrEmpty(email))
            {

                Email newemail = new Email();
                newemail.Name = email;
                db.Emails.Add(newemail);
                db.SaveChanges();


                emailSender.SendEmailAsync(email, "Online Recruitment system greetings", "You will get job alert which is fit on your profile");
                return View();
            }
            return RedirectToAction("Index", "Jobs");
        }

        //public async Task<IActionResult> GetAlert(string str)
        //{
        //    if (!string.IsNullOrEmpty(str))
        //    {
        //        AppUser loggedUser = await userManager.FindByNameAsync(User.Identity.Name);
        //        Candidate candidate = db.Candidates.FirstOrDefault(x => x.AppUserId == loggedUser.Id);
        //        var email = candidate.Email;
        //        Category category = db.Categories.FirstOrDefault(x => x.Name == str);

        //        if (candidate != null && category != null)
        //        {
        //            JobFollowers jfs = new JobFollowers();
        //            jfs.Candidate = candidate;
        //            jfs.Category = category;
        //            db.JobFollowers.Add(jfs);
        //            db.SaveChanges();

        //        }
        //        //var message = new Message(email, "Online Recruitment system greetings", "You will get job alert which is fit on your profile");
        //        //emailSender.SendEmail(message);
        //    }

        //    return RedirectToAction("Index", "Jobs");
        //}
    }
}
