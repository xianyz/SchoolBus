using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using SchoolBusWXWeb.Business;
using SchoolBusWXWeb.Hubs;
using SchoolBusWXWeb.Models;
using Senparc.CO2NET.Extensions;
using Senparc.Weixin;
using Senparc.Weixin.MP.Containers;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable NotAccessedField.Local

namespace SchoolBusWXWeb.Controllers
{
    public class HomeController : Controller
    {
        private static readonly string AppId = Config.SenparcWeixinSetting.WeixinAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
        private readonly SiteConfig _option;
        private readonly ISchoolBusBusines _schoolBusBusines;
        private readonly IHubContext<ChatHub> _chatHub;
        public HomeController(IOptions<SiteConfig> option, ISchoolBusBusines schoolBusBusines, IHubContext<ChatHub> chatHub)
        {
            _chatHub = chatHub;
            _option = option.Value;
            _schoolBusBusines = schoolBusBusines;
        }
        public async Task<IActionResult> Index()
        {
            //await _chatHub.Clients.All.SendAsync("ReceiveMessage", $"刘哲", "你好");
            //string msg= "{\"pkid\":\"4475EEDC2BB74F7C8FA6A59CEDB64720\",\"dev_id\":\"1124347989\",\"sxc_zt\":\"2\",\"jd\":\"123.354068\",\"wd\":\"41.857727\",\"gd\":\"0\",\"card_num\":\"0\"}";
            await _chatHub.Clients.Group("3603631297").SendAsync("ReceiveMessage", 123.373345, 41.874623);
            return View();
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
