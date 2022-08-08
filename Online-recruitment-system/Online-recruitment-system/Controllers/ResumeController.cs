using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_recruitment_system.DAL;
using Online_recruitment_system.Models.Entities;
using Online_recruitment_system.Models.ViewModels;
using Online_recruitment_system.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Online_recruitment_system.Controllers
{

    public class ResumeController : Controller
    {
        private readonly AppDbContext db;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;

        public ResumeController(AppDbContext database, IWebHostEnvironment env, UserManager<AppUser> userManager)
        {
            this.db = database;
            _env = env;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id != 0 || id != null)
            {
                Candidate candidate = await db.Candidates.FirstOrDefaultAsync(m => m.Id == id);

                if (candidate==null)
                {
                    return NotFound();
                }
                Resume res = new Resume();
                res.CandidateId=candidate.Id;

                CvViewModel cvm = new CvViewModel();
                cvm.Candidate = candidate;
                cvm.Resume = res;

                return View(cvm);

            }
            return View();

        }

        public async Task<IActionResult> Show(int? id)
        {
            if (id != null || id != 0)
            {
                ResumeViewModel rvm = new ResumeViewModel();
                Candidate candidate = await db.Candidates.FirstOrDefaultAsync(m => m.Id == id);
                List<Resume> resume = await db.Resumes.Where(x => x.CandidateId == candidate.Id).ToListAsync();

                if (resume.Count() == 0)
                {
                    rvm.Resumes = null;
                    return RedirectToAction("GoUpload","Resume");
                }

                rvm.Candidate = candidate;
                rvm.Resumes = resume;

                foreach (var item in rvm.Resumes)
                {
                    if (!string.IsNullOrEmpty(item.Resume_Uploading))
                    {
                        return View(rvm);
                    }
                    else
                    {
                        return RedirectToAction("Upload");
                    }
                }

            }
            return RedirectToAction("CandidateResume", "Candidates");

        }

        public IActionResult GoUpload()
        {
            return View();

        }

        public async Task<IActionResult> ShowToCompany(int? id)
        {
            if (id != null || id != 0)
            {
                ResumeViewModel rvm = new ResumeViewModel();
                Candidate candidate = await db.Candidates.FirstOrDefaultAsync(m => m.Id == id);
                List<Resume> resume = await db.Resumes.Where(x => x.CandidateId == candidate.Id).ToListAsync();
                rvm.Candidate = candidate;

                if (resume.Count() == 0)
                {
                    rvm.Resumes = null;
                    return RedirectToAction("CvNotFound", "Candidates");

                }
                else
                {
                    rvm.Resumes = resume;
                    foreach (var item in rvm.Resumes)
                    {
                        if (string.IsNullOrEmpty(item.Resume_Uploading))
                        {
                            return RedirectToAction("CvNotFound", "Candidates");
                        }
                     
                    }

                    return View(rvm);

                }

            }
            return RedirectToAction("CandidateResume", "Candidates");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<IActionResult> Upload(CvViewModel cvm)
        {
            if (cvm.Resume.Resume_Upload != null)
            {
                cvm.Resume.Resume_Uploading = await cvm.Resume.Resume_Upload.UpLoad(_env.WebRootPath, @"resumes");
            }

            else
            {
                return RedirectToAction("Index","Resume");
            }

            if (ModelState.IsValid)
            {
                db.Resumes.Add(cvm.Resume);
                await db.SaveChangesAsync();
                return RedirectToAction("CandidateResume","Candidates");
            }

            //if (ModelState.IsValid)
            //{
            //    foreach (var file in Request.Form.Files)
            //    {
            //        if (file.Length == 0)
            //            ModelState.AddModelError("ModelError", "please provide valid file");

            //        var fileName = (model.Resume_Title + "_" + DateTime.Now.ToString("dd_MMM_yyyy_hhmmss") + Path.GetExtension(file.FileName)).Replace(" ", "_");

            //        1) Upload file to any cloud stoarege or database
            //        using (var fileStream = file.OpenReadStream())
            //        using (var ms = new MemoryStream())
            //        {
            //            await fileStream.CopyToAsync(ms);
            //            bool status = await _Service.UploadFileAsync(ms, fileName); // Call Upload Api
            //        }

            //        2) Save file to local path in Resumes folder
            //        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resumes", fileName);
            //        using (var stream = new FileStream(path, FileMode.Create))
            //        {
            //            await file.CopyToAsync(stream);
            //        }


            //        if (ModelState.IsValid)
            //        {
            //            db.Resumes.Add(model);
            //            await db.SaveChangesAsync();
            //            return RedirectToAction(nameof(Index));
            //        }


            //        return Redirect("/resume");
            //    }
            //}
            //else
            //{
            //    ModelState.AddModelError("ModelError", ModelState.FirstOrDefault().Value.Errors.FirstOrDefault().ErrorMessage);
            //}
            return RedirectToAction("CandidateResume", "Candidates");
        }
    }
}
