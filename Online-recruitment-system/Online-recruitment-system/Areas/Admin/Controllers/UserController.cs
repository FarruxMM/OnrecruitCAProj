using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_recruitment_system.DAL;
using Online_recruitment_system.Models.DTO;
using Online_recruitment_system.Models.Entities;
using Online_recruitment_system.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_recruitment_system.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly AppDbContext _context;
        private readonly static int _itemsPerPage = 5;


        public UserController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(int page = 1)
        {
            PaginationModel pgmodel = new PaginationModel()
            {

                ItemsPerPage = _itemsPerPage,
                CurrentPage = page

            };

            List<AppUser> users = await userManager.Users.OrderByDescending(x => x.Id).Skip((page - 1) * _itemsPerPage)
                .Take(_itemsPerPage).ToListAsync();
            pgmodel.TotalItems = userManager.Users.Count();

            List<UserDTO> dto = new List<UserDTO>();
            foreach (AppUser item in users)
            {
                UserDTO user = new UserDTO
                {
                    Id = item.Id,
                    FullName = item.FullName,
                    Email = item.Email,
                    UserName = item.UserName,
                    IsActive = item.IsActive,
                    PaginationModel = pgmodel
                };
                dto.Add(user);
            }

            return View(dto);
        }

        public async Task<IActionResult> ChangeStatus(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index", "User");
            AppUser user = await userManager.FindByIdAsync(id);
            UserDTO dto = new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.UserName,
                IsActive = user.IsActive,

            };

            return View(dto);
        }

        public async Task<IActionResult> ChangeStatusConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index", "User");
            AppUser user = await userManager.FindByIdAsync(id);

            if (user.IsActive)
            {
                user.IsActive = false;
            }
            else
            {
                user.IsActive = true;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "User");
        }


    }
}
