using System.Diagnostics;
using Business.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
       
        public IActionResult Index(HomeIndexVM model)
        {
            return View(model);
        }

    }
}
