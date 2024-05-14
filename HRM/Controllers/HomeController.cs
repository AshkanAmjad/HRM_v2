using Domain.DTOs.Security.Login;
using HRM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace HRM.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
            List<SelectListItem> areas = new();
            areas.Add(new SelectListItem
            {
                Text = "استان",
                Value = 0.ToString()
            });
            areas.Add(new SelectListItem
            {
                Text = "شهرستان",
                Value = 1.ToString()
            });
            areas.Add(new SelectListItem
            {
                Text = "بخش",
                Value = 3.ToString()
            });
            ViewBag.areas = areas;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
