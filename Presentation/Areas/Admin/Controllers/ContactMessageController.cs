using Data.Contexts;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Areas.Admin.Models.ContactMessage;
using Presentation.Areas.Admin.Models.Product;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactMessageController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IContactMessageRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ContactMessageController(AppDbContext context, IContactMessageRepository repository, IUnitOfWork unitOfWork)
        {
            _context = context;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var model = new ContactMessageIndexVM
            {
                ContactMessages = await _repository.GetAllAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var message = await _repository.GetAsync(id);
            if (message == null) return NotFound();

            message.IsRead = true;
            message.ModifiedAt = DateTime.UtcNow;

            _repository.Update(message);
            await _unitOfWork.CommitAsync();

            return Ok();
        }

    }
}
