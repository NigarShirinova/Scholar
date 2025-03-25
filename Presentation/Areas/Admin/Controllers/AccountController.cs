
using Common.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Areas.Admin.Models.Controller;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountLoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email or password is wrong!");
                return Content("<script>alert('Email or password is wrong!'); window.location.href='/Admin/Account/Login';</script>", "text/html");


            }

            if (!_userManager.IsInRoleAsync(user, "Admin").Result)
            {
                ModelState.AddModelError(string.Empty, "You are not admin!");
                return Content("<script>alert('Email or password is wrong!'); window.location.href='/Admin/Account/Login';</script>", "text/html");


            }

            var result = _signInManager.PasswordSignInAsync(user, model.Password, false, false).Result;

            if (!result.Succeeded)
            {

                ModelState.AddModelError(string.Empty, "Unsuccessfull login try!");
                return Content("<script>alert('Email or password is wrong!'); window.location.href='/Admin/Account/Login';</script>", "text/html");

                
            }


            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}
