using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]
    public class DocumentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
