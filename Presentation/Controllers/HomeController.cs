using System.Diagnostics;
using Business.Abstract;
using Business.ViewModels.Contact;
using Business.ViewModels.Home;
using Common.Entities;
using Data.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IContactMessageService _contactMessageService;
        public HomeController(AppDbContext context, IContactMessageService messageService)
        {
            _context = context;
            _contactMessageService = messageService;
        }
        public IActionResult Index()
        { 


            var userName = HttpContext.Session.GetString("UserName");
            var teamMembers = _context.TeamMembers.ToList();


            var model = new HomeIndexVM
            {
                UserName = userName,
                teamMembers = teamMembers,
                ContactMessage = new Business.ViewModels.Contact.ContactMessageCreateVM()

            };

           
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(HomeIndexVM model)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _contactMessageService.CreateMessageAsync(model.ContactMessage);
            return Content("<script>alert('Your Message Sent To Admin!'); window.location.href='/Home';</script>", "text/html");
        }
    }

}
