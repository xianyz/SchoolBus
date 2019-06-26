using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SchoolBusWXWeb.Controllers
{
    public class SchoolBusController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}