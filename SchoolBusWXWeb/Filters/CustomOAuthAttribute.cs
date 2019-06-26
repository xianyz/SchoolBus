using Microsoft.AspNetCore.Http;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.MvcExtension;

namespace SchoolBusWXWeb.Filters
{
    public class CustomOAuthAttribute : SenparcOAuthAttribute
    {
        public CustomOAuthAttribute(string appid,string oauthCallbackUrl, OAuthScope oauthScope = OAuthScope.snsapi_userinfo) : base(appid, oauthCallbackUrl, oauthScope)
        {
            _appId = Config.SenparcWeixinSetting.WeixinAppId; // 如果微信的注册在app.UseMvc下面注册将会回去不到
        }


        public override bool IsLogined(HttpContext httpContext)
        {
            httpContext.Session.SetString("appid", Config.SenparcWeixinSetting.WeixinAppId);
            return httpContext.Session.GetString("Userinfo") != null;
            //return true;
        }
    }
}
