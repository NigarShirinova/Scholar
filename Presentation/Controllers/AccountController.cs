
using Business.Abstract;
using Business.ViewModels.Account;
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

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Login");
        }
    }
}
