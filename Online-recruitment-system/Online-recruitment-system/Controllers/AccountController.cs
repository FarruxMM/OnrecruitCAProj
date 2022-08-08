using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_recruitment_system.Models.Entities;
using Online_recruitment_system.Models.Interface;
using Online_recruitment_system.Models.ViewModels;
using System.Threading.Tasks;

namespace Online_recruitment_system.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IEmailSender _emailSender;

        
        public AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<IdentityRole> _roleManager, IEmailSender emailSender)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
           this._emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Content("Not logged in");
            }

            AppUser loggedUser = await userManager.FindByNameAsync(User.Identity.Name);

            return Json(loggedUser);
        }

        public IActionResult Option()
        {
            return View();
        }

        public IActionResult Login()
        {
            //Javid17
            //Password:Cavid1995@
            //Neriman27
            //Password:Neriman1995@
            //Hakim@mail.ru
            //Hakim1995@
            //Fuad@mail.ru
            //Fuad123@


            //company:Azersun@mail.ru pass:Azersun123@
            //azerpocht@mail.ru pass:Azerpoct123@
            //Socar@mail.ru pass:Socar123@
            //Azal@mail.ru pass:Azal123@
            


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {

            if (!ModelState.IsValid) return View();

            AppUser loggingUser = await userManager.FindByEmailAsync(lvm.Email);


            if (loggingUser == null)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(lvm);

            }

            if (!loggingUser.IsActive)
            {
                ModelState.AddModelError("", "Your Profile has blocked by Admin!Contact with Admin");
                return RedirectToAction("BlockedUser","Account");

                //TempData["Block"] = "Your Account has blocked by Admin! please Contact With Us";
                //return RedirectToAction("Index","Contact");
            }


            var signInResult = await signInManager.PasswordSignInAsync(loggingUser, lvm.Password, true, true);

            if (signInResult.IsLockedOut)
            {

                ModelState.AddModelError("", "You are locked out");

                return View(lvm);
            }

            if (!signInResult.Succeeded)
            {

                ModelState.AddModelError("", "Email or password is wrong");

                return View(lvm);
            }


            //bool available = User.IsInRole("Teacher");

            //if (available)
            //{
            //    string name = loggingUser.FullName;
            //    int found = name.IndexOf(" ");
            //    TempData["name"] = name.Remove(found + 1);
            //    return RedirectToAction("Index", "Teacher" /*, ("/"+name1)*/);
            //}


            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }


        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult BlockedUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel ftpsvm)
        {
            if (string.IsNullOrEmpty(ftpsvm.Email))
            {
                return NotFound();
            }

            AppUser loggedUser = await userManager.FindByEmailAsync(ftpsvm.Email);

            if (loggedUser==null)
            {
                ModelState.AddModelError("","user not found");
                return View(ftpsvm);
            }

            //token 

            var code = await userManager.GeneratePasswordResetTokenAsync(loggedUser);
            var url = Url.Action("ResetPassword", "Account", new
            {
                userId = loggedUser.Id,
                token = code

            }) ;
            //Url

            await _emailSender.SendEmailAsync(ftpsvm.Email,"Reset Password", $"Link for Reseting Password <a href='https://localhost:44360{url}'>Click</a>");
            TempData["Notification1"] = "Reset Password link send to your Email";
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId ,string token)
        {
            if (userId==null || token==null)
            {
                ModelState.AddModelError("","Informations are not correct ");
                return View();
            }
            return View(new ResetPasswordModel { Token = token});
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            var user =await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "User Not Found");
                return View();
            }
            var result =await userManager.ResetPasswordAsync(user,model.Token,model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login","Account" );

            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("",error.Description);
                return View(model);
            }

            return View();
        }


        public IActionResult RegisterCandidate()
        {
            return View();
        }

        public IActionResult RegisterCompany()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterCandidate(RegisterCandidateViewModel rcvm)
        {
            if (!ModelState.IsValid) return View();
            string name = rcvm.Name.Replace(" ", "");

            AppUser newUser = new AppUser
            {
                UserType=rcvm.UserType,
                FullName = rcvm.Name + " " + rcvm.Surname,
                Email = rcvm.Email,
                UserName = name.Trim(),
                IsActive = true
            };

            IdentityResult identityResult = await userManager.CreateAsync(newUser, rcvm.Password);

            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(rcvm);
            }

            await signInManager.SignInAsync(newUser, true);
            ViewBag.Check = "true";


            return RedirectToAction("CreateCandidateProfile", "Candidates");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterCompany(RegisterCompanyViewModel rvm)
        {
            if (!ModelState.IsValid) return View();

            string name = rvm.Name.Replace(" ","");

            AppUser newUser = new AppUser
            {
                FullName = rvm.Name,
                Email = rvm.Email,
                UserType = rvm.UserType,
                UserName = name.Trim(),
                IsActive = true
            };

            IdentityResult identityResult = await userManager.CreateAsync(newUser, rvm.Password);

            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(rvm);
            }

            await signInManager.SignInAsync(newUser, true);

            return RedirectToAction("CreateProfile", "Company");
        }


        //public async Task<IActionResult> InitRoles()
        //{
        //    await roleManager.CreateAsync(new IdentityRole("Member"));
        //    await roleManager.CreateAsync(new IdentityRole("Admin"));
        //    return Content("Okay");
        //}
    }
}
