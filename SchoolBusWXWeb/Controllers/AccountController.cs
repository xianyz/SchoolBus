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
            ViewData["userinfo"] = HttpContext.Session.GetString("OAuthAccessTokenResult");
            return View();
        }
    }
}