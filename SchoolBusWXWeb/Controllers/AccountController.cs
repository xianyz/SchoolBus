using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolBusWXWeb.Filters;

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