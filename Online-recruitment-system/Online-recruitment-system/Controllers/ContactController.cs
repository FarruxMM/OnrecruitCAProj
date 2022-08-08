using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_recruitment_system.DAL;
using Online_recruitment_system.Models.Entities;
using Online_recruitment_system.Models.ViewModels;
using System.Threading.Tasks;

namespace Online_recruitment_system.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext db;

        public ContactController(AppDbContext database)
        {
            this.db = database;
        }
        public async Task<IActionResult> Index()
        {
            ContactViewModel cvm = new ContactViewModel();
            cvm.Contact = await db.Contacts.FirstOrDefaultAsync();
            cvm.Branch = await db.Branches.ToListAsync();
            return View(cvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(Contact contact)
        {

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    Error = false,
                    Message = "muraciet ugurludur"
                });
            }

            //return Json(contact);
            if (!ModelState.IsValid) return RedirectToAction("Index", "Home");



            await db.Contacts.AddAsync(contact);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
