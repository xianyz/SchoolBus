using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolBusWXWeb.Business;

namespace SchoolBusWXWeb.Controllers
{
    public class SchoolBusController : Controller
    {
        private readonly ISchoolBusBusines _schoolBusBusines;
        public SchoolBusController(ISchoolBusBusines schoolBusBusines)
        {
            _schoolBusBusines = schoolBusBusines;
        }
        /// <summary>
        /// https://localhost:5001/schoolbus/Register
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Register()
        {
            var st= await _schoolBusBusines.GetTwxuserAsync();
            return View();
        }
    }
}