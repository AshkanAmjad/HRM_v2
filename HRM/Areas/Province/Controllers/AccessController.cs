using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]
    public class AccessController : Controller
    {
        #region Index
        public IActionResult ProvinceAssistantsIndex()
        {
            return View();
        }

        public IActionResult ProvinceRolesIndex() 
        {
            return View();
        }

        #endregion
    }
}
