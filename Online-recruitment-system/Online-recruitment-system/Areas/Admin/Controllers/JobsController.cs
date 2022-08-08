using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_recruitment_system.DAL;
using Online_recruitment_system.Models.Entities;
using Online_recruitment_system.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Online_recruitment_system.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class JobsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly static int _itemsPerPage = 5;

        public JobsController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(int page = 1)
        {
            JobViewModel jvm = new JobViewModel();
            var data = await _context.Jobs.Include(x => x.Company).Include(x => x.Category).Include(x => x.JobSituation).Include(x => x.JobType)
                .Include(x => x.Position).OrderByDescending(x => x.Id).Skip((page - 1) * _itemsPerPage)
                .Take(_itemsPerPage).ToListAsync();
            var data1 = _context.Jobs.Count();

            jvm.Jobs = data;

            PaginationModel pgmodel = new PaginationModel()
            {
                TotalItems = data1,
                ItemsPerPage = _itemsPerPage,
                CurrentPage = page

            };

            jvm.Pagination = pgmodel;
            return View(jvm);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(j => j.Category)
                .Include(j => j.Company)
                .Include(j => j.JobSituation)
                .Include(j => j.JobType)
                .Include(j => j.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }


        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "About");
            ViewData["JobSituationId"] = new SelectList(_context.JobSituations, "Id", "Name");
            ViewData["JobTypeId"] = new SelectList(_context.JobTypes, "Id", "Type");
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,PositionId,CompanyId,JobTypeId,JobSituationId,Location,CreateDate,Salary,Id")] Job job)
        {
            if (ModelState.IsValid)
            {
                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", job.CategoryId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "About", job.CompanyId);
            ViewData["JobSituationId"] = new SelectList(_context.JobSituations, "Id", "Name", job.JobSituationId);
            ViewData["JobTypeId"] = new SelectList(_context.JobTypes, "Id", "Type", job.JobTypeId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name", job.PositionId);
            return View(job);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs.Include(x=>x.Applications).FirstOrDefaultAsync(x=>x.Id==id);
         
            if (job == null)
            {
                return NotFound();
            }

            //if (job.Applications!=null)
            //{

            //}

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", job.CategoryId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "About", job.CompanyId);
            ViewData["JobSituationId"] = new SelectList(_context.JobSituations, "Id", "Name", job.JobSituationId);
            ViewData["JobTypeId"] = new SelectList(_context.JobTypes, "Id", "Type", job.JobTypeId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name", job.PositionId);
            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", job.CategoryId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "About", job.CompanyId);
            ViewData["JobSituationId"] = new SelectList(_context.JobSituations, "Id", "Name", job.JobSituationId);
            ViewData["JobTypeId"] = new SelectList(_context.JobTypes, "Id", "Type", job.JobTypeId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name", job.PositionId);
            return View(job);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(j => j.Category)
                .Include(j => j.Company)
                .Include(j => j.JobSituation)
                .Include(j => j.JobType)
                .Include(j => j.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Admin/Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(int id)
        {
            return _context.Jobs.Any(e => e.Id == id);
        }
    }
}
