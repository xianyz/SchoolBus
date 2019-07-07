using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SchoolBusWXWeb.Models;
using Senparc.CO2NET.Extensions;
using Senparc.Weixin;
using Senparc.Weixin.MP.Containers;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
#if !DEBUG
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SchoolBusWXWeb.Filters;
#endif
using SchoolBusWXWeb.Business;
using SchoolBusWXWeb.Models.ViewData;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable NotAccessedField.Local

namespace SchoolBusWXWeb.Controllers
{
#if !DEBUG
    [CustomOAuth(null, "/OAuth2/UserInfoCallback")]
#endif
    public class HomeController : Controller
    {
        private static readonly string AppId = Config.SenparcWeixinSetting.WeixinAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
        private readonly SiteConfig _option;
        private readonly ISchoolBusBusines _schoolBusBusines;
        private OAuthAccessTokenResult TokenResult { get;set; }
        public HomeController(IOptions<SiteConfig> option, ISchoolBusBusines schoolBusBusines)
        {
            _option = option.Value;
            _schoolBusBusines = schoolBusBusines;
#if !DEBUG
          TokenResult = JsonConvert.DeserializeObject<OAuthAccessTokenResult>(HttpContext.Session.GetString("OAuthAccessTokenResult"));
#endif
        }
        public async Task<IActionResult> Index(int type)
        {
#if DEBUG
            TokenResult = new OAuthAccessTokenResult {openid = "oBcNx1lHzHxIpKm5m64XX99zTMGs"};
#endif
            var code = await _schoolBusBusines.GetUserCodeAsync(TokenResult.openid);
            return View(new IndexModel { type = type, code = code });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // 客服消息20条限制 如果用户不跟你互动就不能发送
        public async Task<IActionResult> CustomApi(string openid = "ovzSu1Ux_R10fGTWCEawfdVADSy8")
        {
            for (var i = 0; i < 3; i++)
            {
                await Task.Delay(3000);
                try
                {
                    await Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendTextAsync(AppId, openid, "服务器发来的客服消息" + (3 - i));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            var result = await Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendTextAsync(AppId, openid, "服务器发来的客服消息");
            return Content(result.ToJson());
        }

        public async Task<IActionResult> ChangeAccessToken()
        {
            var accessToken = await AccessTokenContainer.GetAccessTokenAsync(AppId);
            return Content(accessToken);
        }
    }
}
