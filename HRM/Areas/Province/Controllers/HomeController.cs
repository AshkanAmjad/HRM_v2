using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]

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

        public IActionResult Management()
        {
            return View();
        }
    }
}
