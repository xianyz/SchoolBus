using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SchoolBusWXWeb.Filters;
using SchoolBusWXWeb.Models.ViewData;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.Helpers;

namespace SchoolBusWXWeb.Controllers
{
    [CustomOAuth(null, "/OAuth2/UserInfoCallback", Senparc.Weixin.MP.OAuthScope.snsapi_userinfo)]
    public class OAuthAndJsSdkController : Controller
    {
        protected OAuthUserInfo UserInfoe { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            UserInfoe =JsonConvert.DeserializeObject<OAuthUserInfo>(context.HttpContext.Session.GetString("Userinfo"));
        }
      
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            if (!(ViewData.Model is Base_JSSDKVD vd)) return;
            var model = vd;
            model.UserInfo = UserInfoe;
            model.PageRenderTime = DateTime.Now;
            model.JsSdkUiPackage= JSSDKHelper.GetJsSdkUiPackage(Config.SenparcWeixinSetting.WeixinAppId, 
                Config.SenparcWeixinSetting.WeixinAppSecret,
                Request.AbsoluteUri());
        }
    }
}
