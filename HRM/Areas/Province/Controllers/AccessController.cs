using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]
    public class AccessController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
