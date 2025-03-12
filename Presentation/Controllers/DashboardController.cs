using System.Security.Claims;
using Business.ViewModels.TeacherDashboard;
using Common.Entities;
using Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private AppDbContext _context;
        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            var userType = "";
            if (user != null)
            {
                var discriminator = _context.Users
                    .Where(u => u.Id == userId)
                    .Select(u => EF.Property<string>(u, "Discriminator"))
                    .FirstOrDefault();

                if (discriminator == "UserTeacher")
                {
                    userType = "Teacher";
                }
                else if (discriminator == "UserStudent")
                {
                    userType = "Student";
                }
            }

            var transactions = _context.Transactions
               .Where(t => t.UserId == userId)
               .OrderByDescending(t => t.DateTime)
               .ToList();

            decimal balance = 0;
            foreach (var transaction in transactions) {
                if (transaction.TransactionType == Common.Constants.TransactionType.Income)
                {
                    balance = balance + transaction.Amount;
                }
                else if(transaction.TransactionType == Common.Constants.TransactionType.Withdrawal)
                {
                    balance = balance - transaction.Amount;
                }
            }
            
            var model = new DashboardIndexVM
            {
                Balance = balance,
                UserType = userType
            };
            return View(model);
        }

        public IActionResult Balance()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            var userType = "";
            if (user != null)
            {
                var discriminator = _context.Users
                    .Where(u => u.Id == userId)
                    .Select(u => EF.Property<string>(u, "Discriminator"))
                    .FirstOrDefault();

                if (discriminator == "UserTeacher")
                {
                    userType = "Teacher";
                }
                else if (discriminator == "UserStudent")
                {
                    userType = "Student";
                }
            }



            var transactions = _context.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.DateTime)
                .ToList();


            decimal balance = 0;
            foreach (var transaction in transactions)
            {
                if (transaction.TransactionType == Common.Constants.TransactionType.Income)
                {
                    balance = balance + transaction.Amount;
                }
                else if (transaction.TransactionType == Common.Constants.TransactionType.Withdrawal)
                {
                    balance = balance - transaction.Amount;
                }
            }



            var model = new BalanceIndexVM
            {
                TransactionList = transactions,
                Balance = balance,
                UserType = userType
            };

            return View(model);
        }
    }
}
