using System.Diagnostics;
using Business.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("UserName");

           

            var model = new HomeIndexVM
            {
                UserName = userName
            };

            return View(model);
        }
    }

}
