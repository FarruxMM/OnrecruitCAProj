using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_recruitment_system.DAL;
using Online_recruitment_system.Models.Entities;
using Online_recruitment_system.Models.ViewModels;
using Online_recruitment_system.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Online_recruitment_system.Controllers
{
    public class CandidatesController : Controller
    {
        private readonly AppDbContext db;
        private readonly UserManager<AppUser> userManager;
        private readonly IWebHostEnvironment env;
        private readonly static int _itemsPerPage = 4;

        public CandidatesController(AppDbContext database, UserManager<AppUser> _userManager, IWebHostEnvironment _env)
        {
            this.db = database;
            this.userManager = _userManager;
            this.env = _env;
        }


        public IActionResult Index(int page = 1)
        {

            FilterCandidateViewModel filterCandidateVM = new FilterCandidateViewModel();

            List<Candidate> candidates = db.Candidates.Include(x => x.Profession).Include(x => x.Educations).Include(x => x.SkillToCandidates)
                .Include(x => x.Experiences).Include(x => x.Resumes)
                .Include(x => x.SocialLinks).OrderByDescending(x => x.Id).Skip((page - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();

            var data1 = db.Candidates.Count();

            PaginationModel pgModel = new PaginationModel()
            {
                TotalItems = data1,
                ItemsPerPage = _itemsPerPage,
                CurrentPage = page
            };

            filterCandidateVM.Candidates = candidates;
            filterCandidateVM.PaginationModel = pgModel;
            ViewBag.Professions = new SelectList(db.Professions.ToList(), "Id", "Name");
            ViewBag.Skills = new SelectList(db.Skills.ToList(), "Id", "Name");
            ViewBag.Locations = new SelectList(db.Locations.ToList(), "Id", "Address");
            return View(filterCandidateVM);
        }

        public IActionResult ApliedJobs(int id)
        {
            List<Application> applications = db.Applications.Where(x => x.CandidateId == id).ToList();

            List<Job> jobs = new List<Job>();

            foreach (var item in applications)
            {
                jobs = db.Jobs.Where(x => x.Id == item.JobId).Include(x => x.Category).Include(x => x.JobType)
                   .Include(x => x.Company).Include(x => x.Position).ToList();
            }

            if (jobs.Count() == 0)
            {
                TempData["NoApliedJob"] = "You don't have any Aplied Job";
            }

            return View(jobs);
        }

        public IActionResult CvNotFound()
        {
            return View();
        }

        public async Task<IActionResult> CandidateResume()
        {

            AppUser loggedUser = await userManager.FindByNameAsync(User.Identity.Name);
            Candidate candidate = await db.Candidates.Include(x => x.Experiences)
                .Include(x => x.Educations).Include(x => x.Profession).Include(x => x.Gender)
                .Include(x => x.SkillToCandidates).ThenInclude(x => x.Skill).FirstOrDefaultAsync(x => x.AppUserId == loggedUser.Id);

            if (candidate == null)
            {
                return RedirectToAction("CreateCandidateProfile", "Candidates");
            }

            return View(candidate);
        }

        public async Task<IActionResult> ViewCandidateResume(int? id)
        {
            if (id == null || id == 0)
            {
                return NoContent();
            }
            Candidate candidate = await db.Candidates.Include(x => x.Experiences).Include(x => x.Educations).Include(x => x.Profession)
                .Include(x => x.SkillToCandidates).ThenInclude(x => x.Skill).Include(x => x.Gender).FirstOrDefaultAsync(x => x.Id == id);

            if (candidate == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewCandidateProfileViewModel vcpvm = new ViewCandidateProfileViewModel();
            vcpvm.Candidate = candidate;

            var skilltocandidate = db.SkillToCandidates.FirstOrDefault(x => x.CandidateId == candidate.Id);
            if (skilltocandidate != null)
            {
                var skillId = skilltocandidate.SkillId;
                var skilltocandidates = db.SkillToCandidates.Where(x => x.SkillId == skillId).ToList();

                List<Candidate> candidates = new List<Candidate>();
                foreach (var item in skilltocandidates)
                {
                    candidates = db.Candidates.Where(x => x.Id == item.CandidateId).ToList();
                }

                vcpvm.Candidates = candidates;
            }
            else
            {
                vcpvm.Candidates = null;
            }

            return View(vcpvm);
        }



        public async Task<IActionResult> CreateCandidateProfile()
        {

            AppUser loggedUser = await userManager.FindByNameAsync(User.Identity.Name);

            CandidateViewModel cvm = new CandidateViewModel()
            {
                AppUser = loggedUser,
                AppUserId = loggedUser.Id,
                SkillList = new SelectList(db.Skills.ToList(), "Id", "Name"),
                Genders = new SelectList(db.Genders.ToList(), "Id", "Name"),
                Professions = new SelectList(db.Professions.ToList(), "Id", "Name")
            };
            TempData["Notification"] = "Profilinize melumatlari elave etmeyiniz xahis olunur";

            return View(cvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCandidateProfile(CandidateViewModel cvm)
        {
            if (cvm.Img != null)
            {
                if (!cvm.Img.IsImage())
                {
                    ModelState.AddModelError("Img", "File Shekil olmalidir");
                    return View();
                }

                if (!cvm.Img.IsValidImage(700))
                {
                    ModelState.AddModelError("Img", "Fayl max 700 kb ola biler!");
                    return View();
                }

                cvm.Image = await cvm.Img.UpLoad(env.WebRootPath, @"img/candidateimg");
            }

            Candidate candidate = new Candidate();
            candidate.Image = cvm.Image;
            candidate.FirstName = cvm.FirstName;
            candidate.LastName = cvm.LastName;
            candidate.AppUserId = cvm.AppUserId;
            candidate.ProfessionId = cvm.ProfessionId;
            candidate.GenderId = cvm.GenderId;
            candidate.BirthDate = cvm.BirthDate;
            candidate.Email = cvm.Email;
            candidate.Usertype = 1;
            candidate.PhoneNumber = cvm.PhoneNumber;
            candidate.About = cvm.About;

            db.Candidates.Add(candidate);
            db.SaveChanges();
            Education edu = new Education();
            edu.University = cvm.Education.University;
            edu.Faculty = cvm.Education.Faculty;
            edu.Degree = cvm.Education.Degree;
            edu.Years = cvm.Education.Years;
            edu.CandidateId = candidate.Id;

            db.Educations.Add(edu);
            db.SaveChanges();

            Experience exp = new Experience();
            exp.Year = cvm.Experience.Year;
            exp.Position = cvm.Experience.Position;
            exp.Location = cvm.Experience.Location;
            exp.CandidateId = candidate.Id;

            db.Experiences.Add(exp);
            db.SaveChanges();

            for (int i = 0; i < cvm.Skill.Count(); i++)
            {
                SkillToCandidate skltcnd = new SkillToCandidate();
                skltcnd.CandidateId = candidate.Id;
                skltcnd.SkillId = cvm.Skill[i];
                db.SkillToCandidates.Add(skltcnd);
                db.SaveChanges();
            }

            foreach (var item in cvm.SocialLinks)
            {
                SocialLink scl = new SocialLink();
                scl.Name = item.Name;
                scl.CandidateId = candidate.Id;
                db.SocialLinks.Add(scl);
                db.SaveChanges();
            }

            TempData["success"] = "Registration is succesfull";
            return RedirectToAction("Index", "Home");

        }


        public async Task<IActionResult> EditCandidateProfile(int? id)
        {
            if (id == 0 || id == null)
            {
                return NoContent();
            }

            AppUser loggedUser = await userManager.FindByNameAsync(User.Identity.Name);
            Candidate candidate = await db.Candidates.Include(x => x.Profession).Include(x => x.SkillToCandidates)
                .Include(x => x.SocialLinks).Include(x => x.JobFollowers).Include(x => x.Gender).Include(x => x.Experiences)
                .Include(x => x.Educations).Include(x => x.Appuser).Include(x => x.Applications)
                .FirstOrDefaultAsync(x => x.Id == id);

            CandidateViewModel cvm = new CandidateViewModel()
            {
                Candidate = candidate,
                AppUser = loggedUser,
                AppUserId = loggedUser.Id,
                SkillList = new SelectList(db.Skills.ToList(), "Id", "Name"),
                Genders = new SelectList(db.Genders.ToList(), "Id", "Name"),
                Professions = new SelectList(db.Professions.ToList(), "Id", "Name")
            };

            return View(cvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCandidateProfile(int id, CandidateViewModel candidatevm)
        {

            if (id != candidatevm.Candidate.Id)
            {
                return NotFound();
            }

            if (candidatevm.Img != null)
            {
                if (!candidatevm.Img.IsImage())
                {
                    ModelState.AddModelError("Img", "File Shekil olmalidir");
                    return View();
                }

                if (!candidatevm.Img.IsValidImage(700))
                {
                    ModelState.AddModelError("Img", "Fayl max 700 kb ola biler!");
                    return View();
                }
                string filepath = Path.Combine(env.WebRootPath, @"img\candidateimg", candidatevm.Candidate.Image);
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }

                candidatevm.Candidate.Image = await candidatevm.Img.UpLoad(env.WebRootPath, @"img/candidateimg");
            }

            if (!(candidatevm.Skill==null || candidatevm.Skill.Count()==0))
            {
                for (int i = 0; i < candidatevm.Skill.Count(); i++)
                {
                    SkillToCandidate skltcnd = new SkillToCandidate();
                    skltcnd.CandidateId = candidatevm.Candidate.Id;
                    skltcnd.SkillId = candidatevm.Skill[i];
                    db.SkillToCandidates.Add(skltcnd);
                    db.SaveChanges();
                }
            }
          


            if (candidatevm.Education != null)
            {
                if (candidatevm.Education.Years != null && candidatevm.Education.Faculty != null && candidatevm.Education.Degree != null && candidatevm.Education.University != null)
                {

                    Education edu = new Education();
                    edu.Years = candidatevm.Education.Years;
                    edu.Faculty = candidatevm.Education.Faculty;
                    edu.University = candidatevm.Education.University;
                    edu.Degree = candidatevm.Education.Degree;
                    edu.CandidateId = candidatevm.Candidate.Id;
                    db.Educations.Add(edu);
                    db.SaveChanges();
                }
            }


            if (candidatevm.Experience != null)
            {
                if (candidatevm.Experience.Location != null && candidatevm.Experience.Position != null && candidatevm.Experience.Year != 0)
                {
                    Experience exp = new Experience();
                    exp.Year = candidatevm.Experience.Year;
                    exp.Position = candidatevm.Experience.Position;
                    exp.Location = candidatevm.Experience.Location;
                    exp.CandidateId = candidatevm.Candidate.Id;

                    db.Experiences.Add(exp);
                    db.SaveChanges();

                }
            }

            if (candidatevm.SocialLinks.Count() != 0)
            {
                for (int i = 0; i < candidatevm.SocialLinks.Count(); i++)
                {
                    if (candidatevm.SocialLinks[i].Name != null)
                    {
                        SocialLink scl = new SocialLink();
                        scl.Name = candidatevm.SocialLinks[i].Name;
                        scl.CandidateId = candidatevm.Candidate.Id;
                        db.SocialLinks.Add(scl);
                        db.SaveChanges();
                    }

                }

            }

            if (ModelState.IsValid)
            {
                db.Candidates.Update(candidatevm.Candidate);
                await db.SaveChangesAsync();

                return RedirectToAction("CandidateResume", "Candidate");
            }
            return View(candidatevm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FilterCandidate(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return RedirectToAction(nameof(Index));
            }

            //db.Candidates.Where(x => x.FirstName == item.Trim());
            //db.Candidates.Where(x => x.LastName == item.Trim());
            //db.Candidates.Where(x => x.Profession.Name == item.Trim());

            return View();

        }


    }

}
