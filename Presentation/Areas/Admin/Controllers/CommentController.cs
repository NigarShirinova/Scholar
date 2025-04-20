using Data.Contexts;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Areas.Admin.Models.Comment;
using Presentation.Areas.Admin.Models.ContactMessage;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CommentController : Controller
    {
      
        private readonly ICommentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CommentController(AppDbContext context, ICommentRepository repository, IUnitOfWork unitOfWork)
        {
          
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var model = new CommentIndexVM
            {
                Comments = await _repository.GetAllAsync()
            };
            return View(model);
        }

        public async Task<IActionResult> Show(int id)
        {
           var comment = await _repository.GetAsync(id);
            comment.Show = true;
            var model = new CommentIndexVM
            {
                Comments = await _repository.GetAllAsync()
            };
            await _unitOfWork.CommitAsync();
            return RedirectToAction("Index", "Comment", new { area = "Admin" });
        }

        public async Task<IActionResult> Hide(int id)
        {
            var comment = await _repository.GetAsync(id);
            comment.Show = false;
            var model = new CommentIndexVM
            {
                Comments = await _repository.GetAllAsync()
            };
            await _unitOfWork.CommitAsync();
            return RedirectToAction("Index", "Comment", new { area = "Admin" });
        }
    }
}
