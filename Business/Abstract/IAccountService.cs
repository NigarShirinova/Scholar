using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.ViewModels.Account;
using Common.Entities;
using Microsoft.AspNetCore.Identity;

namespace Business.Abstract
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterUserAsync(AccountRegisterVM model);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        void SendConfirmationEmail(string email, string url);
        Task<User> FindUserByEmailAsync(string email);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        Task<SignInResult> LoginUserAsync(AccountLoginVM model);
        Task LogoutAsync();
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);
        void SendPasswordResetEmail(string email, string url);
        Task<string> GeneratePasswordResetTokenAsync(User user);
    }
}
