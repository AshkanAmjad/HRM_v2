using Data.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]
    [AreaPermissionChecker("0")]

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
