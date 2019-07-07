using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
#if !DEBUG
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SchoolBusWXWeb.Filters;
#endif
using SchoolBusWXWeb.Business;
using SchoolBusWXWeb.Models.ViewData;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;

namespace SchoolBusWXWeb.Controllers
{
#if !DEBUG
    [CustomOAuth(null, "/OAuth2/UserInfoCallback")]
#endif
    public class SchoolBusHomeController : Controller
    {
        private readonly ISchoolBusBusines _schoolBusBusines;
        private OAuthAccessTokenResult TokenResult { get; set; }
        public SchoolBusHomeController(ISchoolBusBusines schoolBusBusines)
        {
            _schoolBusBusines = schoolBusBusines;
#if !DEBUG
          TokenResult = JsonConvert.DeserializeObject<OAuthAccessTokenResult>(HttpContext.Session.GetString("OAuthAccessTokenResult"));
#endif
        }
        public async Task<IActionResult> Index(int type)
        {
#if DEBUG
            TokenResult = new OAuthAccessTokenResult { openid = "oBcNx1lHzHxIpKm5m64XX99zTMGs" };
#endif
            var code = await _schoolBusBusines.GetUserCodeAsync(TokenResult.openid);
            return View(new IndexModel { type = type, code = code });
        }
    }
}