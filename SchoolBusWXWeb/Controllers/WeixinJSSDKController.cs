using Microsoft.AspNetCore.Mvc;
using SchoolBusWXWeb.Business;
using SchoolBusWXWeb.Models.ViewData;
// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Controllers
{
    public class WeixinJSSDKController : OAuthBaseController
    {
        public WeixinJSSDKController(ISchoolBusBusines schoolBusBusines) : base(schoolBusBusines) { }
        public IActionResult Index()
        {
            var vd = new JSSDK_Index
            {
                Msg = "当前用户OpenId：" + TokenResult.openid
            };
            return View(vd);
        }
    }
}