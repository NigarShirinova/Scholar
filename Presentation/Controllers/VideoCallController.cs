using Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{

    [Authorize]
    public class VideoCallController : Controller
    {
        private readonly AppDbContext _context;

        public VideoCallController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Join(int lessonId)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var lesson = _context.Lessons
                .Where(l => l.Id == lessonId)
                .Select(l => new
                {
                    l.Id,
                    TeacherId = l.Teacher.Id,
                    StudentId = l.Student.Id
                })
                .FirstOrDefault();

            if (lesson == null || (lesson.TeacherId != userId && lesson.StudentId != userId))
                return Unauthorized();

            var remoteUserId = lesson.TeacherId == userId ? lesson.StudentId : lesson.TeacherId;

            ViewBag.RemoteUserId = remoteUserId;
            return View("Index");
        }
    }
}

