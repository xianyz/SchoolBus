using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolBusWXWeb.Controllers;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.MvcExtension;

namespace SchoolBusWXWeb.Filters
{
    public class CustomOAuthAttribute : SenparcOAuthAttribute
    {
        public CustomOAuthAttribute(string appid, string oauthCallbackUrl, OAuthScope oauthScope = OAuthScope.snsapi_userinfo) : base(appid, oauthCallbackUrl, oauthScope)
        {
            _appId = Config.SenparcWeixinSetting.WeixinAppId;  // 如果微信的注册在app.UseMvc下面注册将会回 取不到
        }


        public override bool IsLogined(HttpContext httpContext)
        {
            return httpContext.Session.GetString("Userinfo") != null;
        }
    }
}
