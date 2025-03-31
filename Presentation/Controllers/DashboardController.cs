using System.Security.Claims;
using Business.Abstract;
using Business.Services;
using Business.ViewModels.TeacherDashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = HttpContext.Session.GetString("Email");
            var model = await _dashboardService.GetDashboardDataAsync(userId, email);
            return View(model);
        }

        public async Task<IActionResult> Balance()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await _dashboardService.GetBalanceDataAsync(userId);
            return View(model);
        }

        [HttpGet]
        public IActionResult AddLesson()
        {
            return View(new LessonCreateVM());
        }

        [HttpPost]
        public async Task<IActionResult> AddLesson(LessonCreateVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!await _dashboardService.AddLessonAsync(userId, model))
            {
                return Content("<script>alert('You did something wrong'); window.location.href='/Dashboard/AddLesson';</script>", "text/html");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> BuyLesson()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userType = await _dashboardService.GetUserTypeAsync(userId);
            var model = new LessonVM
            {
                Lessons = await _dashboardService.GetAvailableLessonsAsync(),
                UserType = userType,
                UserId = userId
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyLessons()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userType = await _dashboardService.GetUserTypeAsync(userId);
            var model = new LessonVM
            {
                Lessons = await _dashboardService.GetUsersLessonsAsync(userId),
                UserType = userType,
                UserId = userId
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> BuyLesson(int lessonId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!await _dashboardService.BuyLessonAsync(userId, lessonId))
            {
                return Content("<script>alert('Your Balance is not sufficient'); window.location.href='/Dashboard/BuyLesson';</script>", "text/html");
            }
            return RedirectToAction("BuyLesson");
        }
    }
}
