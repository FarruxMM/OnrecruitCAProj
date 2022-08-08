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

    public class CompaniesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly static int _itemsPerPage = 5;

        public CompaniesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            CompanyViewModel cmpvm = new CompanyViewModel();

            var data = await _context.Companies.Include(x => x.Location).Include(x => x.Appuser).Include(x => x.CompanyImages)
              .OrderByDescending(x => x.Id).Skip((page - 1) * _itemsPerPage)
              .Take(_itemsPerPage).ToListAsync();
            var data1 = _context.Companies.Count();

            cmpvm.Companies = data;

            PaginationModel pgmodel = new PaginationModel()
            {
                TotalItems = data1,
                ItemsPerPage = _itemsPerPage,
                CurrentPage = page

            };

            cmpvm.Pagination = pgmodel;

            return View(cmpvm);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .Include(c => c.Appuser)
                .Include(c => c.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Address");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,About,Email,AppUserId,PhoneNumber,LocationId,Usertype,Image,Id,IsActive")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", company.AppUserId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Address", company.LocationId);
            return View(company);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", company.AppUserId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Address", company.LocationId);
            return View(company);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,About,Email,AppUserId,PhoneNumber,LocationId,Usertype,Image,Id,IsActive")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", company.AppUserId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Address", company.LocationId);
            return View(company);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .Include(c => c.Appuser)
                .Include(c => c.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
