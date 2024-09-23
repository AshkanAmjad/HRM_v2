using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.County.Controllers
{
    [Area("County")]
    public class HomeController : Controller
    {
        public IActionResult Main()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
