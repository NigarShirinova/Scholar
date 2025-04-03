using Common.Constants;
using Common.Entities;
using Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace Presentation.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public PaymentController(UserManager<User> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> IncreaseBalance(decimal amount)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            if (amount <= 0)
            {
                return BadRequest(new { error = "Amount must be greater than zero." });
            }

            user = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (user == null) return NotFound();

            var transaction = new Transaction
            {
                TransactionType = TransactionType.Income,
                DateTime = DateTime.UtcNow,
                UserId = user.Id,
                PaymentToken = Guid.NewGuid(),
                Amount = amount,
                OrderStatus = OrderStatus.Pending,
            };
            user.Balance = user.Balance + transaction.Amount;
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var successUrl = $"{baseUrl}{Url.Action("Success", "Payment", new { token = transaction.PaymentToken })}";
            var cancelUrl = $"{baseUrl}{Url.Action("Cancel", "Payment", new { token = transaction.PaymentToken })}";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    UnitAmount = (long)(amount * 100),
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Balance Top-up"
                    }
                },
                Quantity = 1
            }
        }
            };


            try
            {
                var service = new SessionService();
                Session session = await service.CreateAsync(options);


                return Json(new { id = session.Id});
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> WithdrawBalance(decimal amount)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            if (amount <= 0)
            {
                return BadRequest(new { error = "Amount must be greater than zero." });
            }

            if (user.Balance < amount)
            {
                return BadRequest(new { error = "Insufficient balance." });
            }

            user = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (user == null) return NotFound();

            var transaction = new Transaction
            {
                TransactionType = TransactionType.Withdrawal,
                DateTime = DateTime.UtcNow,
                UserId = user.Id,
                PaymentToken = Guid.NewGuid(),
                Amount = amount
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
   
            try
            {
               
                user.Balance -= amount;
                _context.Users.Update(user);
                transaction.OrderStatus = OrderStatus.Success;
                _context.Transactions.Update(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction("Balance", "Dashboard");
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }


        public async Task<IActionResult> Success(Guid token)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized(new { error = "User not found." });

      
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.PaymentToken == token &&
                                          t.TransactionType == TransactionType.Income &&
                                          t.UserId == user.Id);

            if (transaction == null) return NotFound(new { error = "Transaction not found." });

    
            if (transaction.OrderStatus == OrderStatus.Success)
            {
                return Ok(new { message = "Transaction already processed." });
            }

            transaction.OrderStatus = OrderStatus.Success;
            _context.Transactions.Update(transaction);

            user.Balance += transaction.Amount;

            await _context.SaveChangesAsync();

            return View();
        }

        public async Task<IActionResult> Cancel(Guid token)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized(new { error = "User not found." });

            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.PaymentToken == token &&
                                          t.TransactionType == TransactionType.Income &&
                                          t.UserId == user.Id);

            if (transaction == null) return NotFound(new { error = "Transaction not found." });

            if (transaction.OrderStatus == OrderStatus.Failure || transaction.OrderStatus == OrderStatus.Success)
            {
                return Ok(new { message = "Transaction already processed." });
            }
            transaction.OrderStatus = OrderStatus.Failure;
            _context.Transactions.Update(transaction);
            var userAccount = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (userAccount != null)
            {
                userAccount.Balance -= transaction.Amount;
                _context.Users.Update(userAccount);
            }
            await _context.SaveChangesAsync();

            return View();
        }

    }
}