using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web_UI.Models;

namespace Web_UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Credits()
        {
            return View();
        }
    }
}