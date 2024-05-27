using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]
    public class ArchiveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
