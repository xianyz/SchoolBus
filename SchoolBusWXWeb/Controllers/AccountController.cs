using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolBusWXWeb.Filters;
using Senparc.Weixin;

namespace SchoolBusWXWeb.Controllers
{
    public class AccountController : Controller
    {

        [CustomOAuth(null, "/OAuth2/UserInfoCallback")]
        public IActionResult Index()
        {
            ViewData["State"] = HttpContext.Session.GetString("State");
            ViewData["userinfo"] = HttpContext.Session.GetString("Userinfo");
            return View();
        }
    }
}