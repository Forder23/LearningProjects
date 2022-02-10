using AppointmentSchedule.API.Database;
using AppointmentSchedule.API.Helper_Extension;
using AppointmentSchedule.API.Models;
using AppointmentSchedule.API.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSchedule.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _DbContext;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _DbContext = context;

            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        //Page Login GET
        public IActionResult Login()
        {
            return View();
        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(LoginViewModel Model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Model.Email, Model.Password, Model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Appointment");
                }
                ModelState.AddModelError("", "Entered wrong email or password or both!");
            }
            return View("Login",Model);
        }

        public async Task<IActionResult> Register()
        {
            if (!_roleManager.RoleExistsAsync(Helper_Extension.Helper.Admin).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(Helper.Admin));
                await _roleManager.CreateAsync(new IdentityRole(Helper.Doctor));
                await _roleManager.CreateAsync(new IdentityRole(Helper.Patient));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(RegisterViewModel Model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser _User = new ApplicationUser
                {
                    UserName = Model.Email,
                    Email = Model.Email,
                    Name = Model.FirstName
                };

                var NewUser = await _userManager.CreateAsync(_User, Model.Password);

                if (NewUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(_User,Model.RoleName);
                    if (!User.IsInRole(Helper.Admin))
                    {
                        await _signInManager.SignInAsync(_User, isPersistent: false);
                    }
                    return RedirectToAction("Index", "Appointment");
                }
                foreach (var error in NewUser.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View("Register",Model);
        }

        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
