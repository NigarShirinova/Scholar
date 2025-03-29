using Common.Entities;
using Data.Contexts;
using Data.Repositories.Abstract;
using Data.Repositories.Base;
using Data.Repositories.Concrete;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Areas.Admin.Models.TeamMember;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TeamMemberController : Controller
    {


        private readonly AppDbContext _context;
        private readonly ITeamMemberRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public TeamMemberController(AppDbContext context, ITeamMemberRepository repository, IUnitOfWork unitOfWork)
        {
            _context = context;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var model = new TeamMemberIndexVM
            {
                TeamMembers = await _repository.GetAllAsync()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeamMemberCreateVM model)
        {
            if(_repository.GetAllAsync().Result.Count >= 4) return Content("<script>alert('Maximum count of team members added!'); window.location.href='/Admin/TeamMember';</script>", "text/html");
            if (!ModelState.IsValid) return View(model);

            string uniqueFileName = null;

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(fileStream);
                }
            }

            var teamMember = new TeamMember
            {
                FullName = model.FullName,
                Position = model.Position,
                PhotoName = uniqueFileName, 
                LinkedinLink = model.LinkedinLink,
                FacebookLink = model.FacebookLink,
                InstagramLink = model.InstagramLink,
                CreatedAt = DateTime.UtcNow,
            };


            await _repository.CreateAsync(teamMember);
            await _unitOfWork.CommitAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            var teamMember = _repository.GetAsync(id);


            var model = new TeamMemberUpdateVM
            {
                FullName = teamMember.Result.FullName,
                Position = teamMember.Result.Position,
                LinkedinLink = teamMember.Result.LinkedinLink,
                FacebookLink = teamMember.Result.FacebookLink,
                InstagramLink = teamMember.Result.InstagramLink,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, TeamMemberUpdateVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var teamMember = await _repository.GetAsync(id);

            
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(fileStream);
                }

             
                if (!string.IsNullOrEmpty(teamMember.PhotoName))
                {
                    var oldPath = Path.Combine(uploadsFolder, teamMember.PhotoName);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                teamMember.PhotoName = uniqueFileName;
            }

           
            teamMember.ModifiedAt = DateTime.UtcNow;
            teamMember.FullName = model.FullName;
            teamMember.Position = model.Position;
            teamMember.LinkedinLink = model.LinkedinLink;
            teamMember.FacebookLink = model.FacebookLink;
            teamMember.InstagramLink = model.InstagramLink;

            _repository.Update(teamMember);
            await _unitOfWork.CommitAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var teamMember = _repository.GetAsync(id);
            if (teamMember is null) return NotFound();

            _repository.Delete(teamMember.Result);
            _unitOfWork.CommitAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
