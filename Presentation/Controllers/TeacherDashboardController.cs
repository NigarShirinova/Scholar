using System.Security.Claims;
using Business.ViewModels.TeacherDashboard;
using Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [Authorize]
    public class TeacherDashboardController : Controller
    {
        private AppDbContext _context;
        public TeacherDashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
            
            var model = new TeacherDashboardIndexVM
            {
                Balance = balance,
            };
            return View(model);
        }

        public IActionResult Balance()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);



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
                Balance = balance
            };

            return View(model);
        }
    }
}
