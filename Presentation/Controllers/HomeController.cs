using System.Diagnostics;
using System.Security.Claims;
using Business.Abstract;
using Business.Concrete;
using Business.Services;
using Business.ViewModels.Comment;
using Business.ViewModels.Contact;
using Business.ViewModels.Home;
using Business.ViewModels.TeacherDashboard;
using Common.Entities;
using Data.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IContactMessageService _contactMessageService;
        private readonly ICommentService _commentService;
        public HomeController(AppDbContext context, IContactMessageService messageService, ICommentService commentService)
        {
            _context = context;
            _contactMessageService = messageService;
            _commentService = commentService;
        }
        public IActionResult Index()
        { 


            var userName = HttpContext.Session.GetString("UserName");
            var teamMembers = _context.TeamMembers.ToList();
            var comments = _context.Comments.ToList();


            var model = new HomeIndexVM
            {
                UserName = userName,
                teamMembers = teamMembers,
                ContactMessage = new Business.ViewModels.Contact.ContactMessageCreateVM(),
                Comments = comments

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

        [HttpPost]
        public async Task<IActionResult> AddComment(CommentCreateVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!await _commentService.AddCommentAsync(model))
            {
                return Content("<script>alert('You did something wrong'); window.location.href='/Dashboard/AddLesson';</script>", "text/html");
            }
            return RedirectToAction("Index");
        }


    }

}
