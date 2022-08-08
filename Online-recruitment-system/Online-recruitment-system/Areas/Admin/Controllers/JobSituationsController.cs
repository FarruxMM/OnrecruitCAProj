using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_recruitment_system.DAL;
using Online_recruitment_system.Models.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Online_recruitment_system.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class JobSituationsController : Controller
    {
        private readonly AppDbContext _context;

        public JobSituationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/JobSituations
        public async Task<IActionResult> Index()
        {
            return View(await _context.JobSituations.ToListAsync());
        }

        // GET: Admin/JobSituations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobSituation = await _context.JobSituations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobSituation == null)
            {
                return NotFound();
            }

            return View(jobSituation);
        }

        // GET: Admin/JobSituations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/JobSituations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id")] JobSituation jobSituation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobSituation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobSituation);
        }

        // GET: Admin/JobSituations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobSituation = await _context.JobSituations.FindAsync(id);
            if (jobSituation == null)
            {
                return NotFound();
            }
            return View(jobSituation);
        }

        // POST: Admin/JobSituations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id")] JobSituation jobSituation)
        {
            if (id != jobSituation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobSituation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobSituationExists(jobSituation.Id))
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
            return View(jobSituation);
        }

        // GET: Admin/JobSituations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobSituation = await _context.JobSituations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobSituation == null)
            {
                return NotFound();
            }

            return View(jobSituation);
        }

        // POST: Admin/JobSituations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobSituation = await _context.JobSituations.FindAsync(id);
            _context.JobSituations.Remove(jobSituation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobSituationExists(int id)
        {
            return _context.JobSituations.Any(e => e.Id == id);
        }
    }
}
