using System;
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
    public class SchoolBusHomeController : ControllerEx
    {
        private readonly ISchoolBusBusines _schoolBusBusines;
        public SchoolBusHomeController(ISchoolBusBusines schoolBusBusines)
        {
            _schoolBusBusines = schoolBusBusines;
        }
        public async Task<IActionResult> Index(int type)
        {

            try
            {
#if DEBUG
                var tokenResult = new OAuthAccessTokenResult { openid = "oBcNx1lHzHxIpKm5m64XX99zTMGs" };
#else 
                var tokenResult = JsonConvert.DeserializeObject<OAuthAccessTokenResult>(HttpContext.Session.GetString("OAuthAccessTokenResult"));
#endif
                ViewData["OpenId"] = tokenResult.openid;
                var code = await _schoolBusBusines.GetUserCodeAsync(tokenResult.openid);
                switch (type)
                {
                    case 0 when (code == 1 || code == 4 || code == 5):
                        return  RedirectToLocal("/SchoolBus/Register");
                    case 1 when (code == 2 || code == 3):
                        return RedirectToLocal("/SchoolBus/GoReport");
                    case 2 when code == 3:
                        return RedirectToLocal("/SchoolBus/GoCardInfo");
                    case 3 when code == 3:
                        return RedirectToLocal("/SchoolBus/GoAddress?showType=1");
                    case 4 when code == 3:
                        return RedirectToLocal("/SchoolBus/GoCalendar");
                    case 5 when code != 1:
                        return RedirectToLocal("/SchoolBus/GoUntying");
                }

                return View(new IndexModel { type = type, code = code });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ViewData["OpenId"] = e.ToString();
            }
            return View(new IndexModel { type = type, code = -1 });
        }
    }
}