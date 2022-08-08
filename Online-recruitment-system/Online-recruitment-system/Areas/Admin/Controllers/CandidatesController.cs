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
    public class CandidatesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly static int _itemsPerPage = 5;

        public CandidatesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page=1)
        {
            CandidateViewModel cvm = new CandidateViewModel();

            var data = await _context.Candidates.Include(x => x.Applications).Include(x => x.Appuser).Include(x => x.Educations).Include(x => x.Experiences)
                .Include(x => x.Gender).Include(x => x.Profession).Include(x => x.SocialLinks).Include(x => x.SkillToCandidates)
                .OrderByDescending(x => x.Id).Skip((page - 1) * _itemsPerPage)
                .Take(_itemsPerPage).ToListAsync();
            var data1 = _context.Candidates.Count();

            cvm.Candidates = data;

            PaginationModel pgmodel = new PaginationModel()
            {
                TotalItems = data1,
                ItemsPerPage = _itemsPerPage,
                CurrentPage = page

            };

            cvm.Pagination = pgmodel;

            return View(cvm);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidates
                .Include(c => c.Appuser)
                .Include(c => c.Gender)
                .Include(c => c.Profession)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name");
            ViewData["ProfessionId"] = new SelectList(_context.Professions, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,AppUserId,Usertype,BirthDate,GenderId,Email,PhoneNumber,About,ProfessionId,Image,Id,IsActive")] Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(candidate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", candidate.AppUserId);
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", candidate.GenderId);
            ViewData["ProfessionId"] = new SelectList(_context.Professions, "Id", "Name", candidate.ProfessionId);
            return View(candidate);
        }

        // GET: Admin/Candidates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", candidate.AppUserId);
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", candidate.GenderId);
            ViewData["ProfessionId"] = new SelectList(_context.Professions, "Id", "Name", candidate.ProfessionId);
            return View(candidate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,AppUserId,Usertype,BirthDate,GenderId,Email,PhoneNumber,About,ProfessionId,Image,Id,IsActive")] Candidate candidate)
        {
            if (id != candidate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(candidate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidateExists(candidate.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", candidate.AppUserId);
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", candidate.GenderId);
            ViewData["ProfessionId"] = new SelectList(_context.Professions, "Id", "Name", candidate.ProfessionId);
            return View(candidate);
        }

        // GET: Admin/Candidates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidates
                .Include(c => c.Appuser)
                .Include(c => c.Gender)
                .Include(c => c.Profession)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        // POST: Admin/Candidates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidateExists(int id)
        {
            return _context.Candidates.Any(e => e.Id == id);
        }
    }
}
