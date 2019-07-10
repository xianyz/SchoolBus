using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SchoolBusWXWeb.Filters;
using SchoolBusWXWeb.Models.ViewData;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.Helpers;
using System;
using System.Threading.Tasks;

namespace SchoolBusWXWeb.Controllers
{
    [CustomOAuth(null, "/OAuth2/UserInfoCallback")]
    public class OAuthBaseController : ControllerEx
    {
        protected OAuthAccessTokenResult TokenResult { get; private set; }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // do something before the action executes
            TokenResult = JsonConvert.DeserializeObject<OAuthAccessTokenResult>(context.HttpContext.Session.GetString("OAuthAccessTokenResult"));
            //context.Result = new RedirectResult("~/Home/Index"); return
            await next();
            // do something after the action executes
            if (!(ViewData.Model is Base_JSSDKVD vd)) return;
            var model = vd;
            model.TokenResult = TokenResult;
            model.PageRenderTime = DateTime.Now;
            model.JsSdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(Config.SenparcWeixinSetting.WeixinAppId,
                Config.SenparcWeixinSetting.WeixinAppSecret,
                Request.AbsoluteUri());
        }
    }
}
