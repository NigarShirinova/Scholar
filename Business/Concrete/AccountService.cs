using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.ViewModels.Account;
using Business.ViewModels.Email;
using Business.ViewModels.Home;
using Common.Entities;
using Microsoft.AspNetCore.Identity;

namespace Business.Concrete
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        public async Task<IdentityResult> RegisterUserAsync(AccountRegisterVM model)
        {
            if (model.Role == "UserStudent")
            {
                var user = new UserStudent
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,

                };
                return await _userManager.CreateAsync(user, model.Password);
            }
            else
            {
                var user = new UserTeacher
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,

                };
                 return await _userManager.CreateAsync(user, model.Password);
            }


               
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public void SendConfirmationEmail(string email, string url)
        {
            string body = $@"
    <html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
                color : white;
                background-color: #f4f4f4;
                padding: 20px;
                text-align: center;
            }}
            .container {{
                background-color: #7a6ad8;
                padding: 20px;
                border-radius: 5px;
                box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
                display: inline-block;
            }}
            .button {{
                display: inline-block;
                padding: 10px 20px;
                font-size: 16px;
                color: #ffffff;
                background-color: rgba(255,255,255,0.13);
                text-decoration: none;
                border-radius: 5px;
            }}
            .button:hover {{
                background-color: rgba(255,255,255,0.50);
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <h2>Confirm Your Account</h2>
            <p>Please click the button for confirmation:</p>
            <a href='{url}' class='button'>Confirm</a>
        </div>
    </body>
    </html>";

            _emailService.SendMessage(new Message(new List<string> { email }, "Account Confirmation", body));
           

        }


        public async Task<User> FindUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<SignInResult> LoginUserAsync(AccountLoginVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user)) return SignInResult.Failed;
            model.FullName = user.FullName;
            return await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public void SendPasswordResetEmail(string email, string url)
        {
            string body = $@"
    <html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
                color : white;
                background-color: #f4f4f4;
                padding: 20px;
                text-align: center;
            }}
            .container {{
                background-color: #7a6ad8;
                padding: 20px;
                border-radius: 5px;
                box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
                display: inline-block;
            }}
            .button {{
                display: inline-block;
                padding: 10px 20px;
                font-size: 16px;
                color: #ffffff;
                background-color: rgba(255,255,255,0.13);
                text-decoration: none;
                border-radius: 5px;
            }}
            .button:hover {{
                background-color: rgba(255,255,255,0.50);
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <h2>Reset Your Password</h2>
            <p>Click the button below to reset your password:</p>
            <a href='{url}' class='button'>Reset Password</a>
        </div>
    </body>
    </html>";

            _emailService.SendMessage(new Message(new List<string> { email }, "Password Reset", body));
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

    }
}
