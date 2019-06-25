using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin;
// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Controllers
{
    public class WeixinJSSDKController : Controller
    {
        private readonly string appId = Config.SenparcWeixinSetting.WeixinAppId;
        private readonly string appSecret = Config.SenparcWeixinSetting.WeixinAppSecret;
        public async Task<IActionResult> Index()
        {
            var jssdkUiPackage = await JSSDKHelper.GetJsSdkUiPackageAsync(appId, appSecret, Request.AbsoluteUri());
            //ViewData["JsSdkUiPackage"] = jssdkUiPackage;
            //ViewData["AppId"] = appId;
            //ViewData["Timestamp"] = jssdkUiPackage.Timestamp;
            //ViewData["NonceStr"] = jssdkUiPackage.NonceStr;
            //ViewData["Signature"] = jssdkUiPackage.Signature;
            return View(jssdkUiPackage);
        }
    }
}