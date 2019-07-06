using Microsoft.AspNetCore.Mvc;
using SchoolBusWXWeb.Models.ViewData;
using Senparc.CO2NET.Extensions;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;

// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Controllers
{
    public class WeixinJSSDKController : OAuthAndJsSdkController
    {
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