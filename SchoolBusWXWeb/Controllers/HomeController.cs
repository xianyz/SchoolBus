using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SchoolBusWXWeb.Models;

namespace SchoolBusWXWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly SiteConfig _option;
        public HomeController(IOptions<SiteConfig> option)
        {
            _option = option.Value;
        }
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
