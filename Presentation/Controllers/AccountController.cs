
using Business.Abstract;
using Business.ViewModels.Account;
using Business.ViewModels.Home;
using Business.ViewModels.TeacherDashboard;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountRegisterVM model)
        {
            if (!ModelState.IsValid)
            {
               
                return View(model);
            }

            var result = await _accountService.RegisterUserAsync(model);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return Content("<script>alert('You registrated with this email before, please try to login!'); window.location.href='/Account/Register';</script>", "text/html");
                }
                return View(model);
            }

            var user = await _accountService.FindUserByEmailAsync(model.Email);
            var token = await _accountService.GenerateEmailConfirmationTokenAsync(user);
            var url = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);
            _accountService.SendConfirmationEmail(user.Email, url);
            return Content("<script>alert('Confirmation email has been sent!'); window.location.href='/Account/Register';</script>", "text/html");
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return NotFound("We wouldn't do that action!");
            }

            var user = await _accountService.FindUserByEmailAsync(email);
            if (user == null) return NotFound();

            var result = await _accountService.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return View("Error", result.Errors);
            }

            return RedirectToAction(nameof(Login));
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

            var result = await _accountService.LoginUserAsync(model);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email or password is wrong!");
                return Content("<script>alert('Email or password is wrong!'); window.location.href='/Account/Login';</script>", "text/html");
            }
            HomeIndexVM homeIndexVM = new HomeIndexVM()
            {
                UserName = model.FullName,

            };
            HttpContext.Session.SetString("Email", model.Email);

            HttpContext.Session.SetString("UserName", model.FullName);


            return RedirectToAction("Index", "Home", homeIndexVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await _accountService.LogoutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _accountService.FindUserByEmailAsync(model.Email);
            if (user == null)
            {
                return Content("<script>alert('If the email exists, a reset link has been sent!'); window.location.href='/Account/Login';</script>", "text/html");
            }

            var token = await _accountService.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);
            _accountService.SendPasswordResetEmail(user.Email, url);

            return Content("<script>alert('Password reset link has been sent!'); window.location.href='/Account/Login';</script>", "text/html");
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return NotFound("Invalid password reset request.");
            }

            var model = new ResetPasswordVM { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _accountService.FindUserByEmailAsync(model.Email);
            if (user == null)
            {
                return Content("<script>alert('User not found!'); window.location.href='/Account/Login';</script>", "text/html");
            }

            var result = await _accountService.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Error resetting password.");
                return View(model);
            }

            return Content("<script>alert('Password has been reset successfully!'); window.location.href='/Account/Login';</script>", "text/html");
        }

    }
}
