using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Extensions;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Threading.Tasks;

// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Controllers
{
    public class OAuth2Controller : Controller
    {
        private readonly string appId = Config.SenparcWeixinSetting.WeixinAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
        private readonly string appSecret = Config.SenparcWeixinSetting.WeixinAppSecret;//与微信公众账号后台的AppId设置保持一致，区分大小写。

        /// <summary>
        /// http://wx.360wll.cn/OAuth2
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult Index(string returnUrl = "http://www.baidu.com")
        {
            var state = "Liuzhe-" + SystemTime.Now.Millisecond;//随机数，用于识别请求可靠性
            //var callbackUrl = "http://wx.360wll.cn/OAuth2/UserInfoCallback?returnUrl=" + returnUrl.UrlEncode();
            var callbackUrl = "http://19ce889a.ngrok.io/OAuth2/UserInfoCallback?returnUrl=" + returnUrl.UrlEncode();
            HttpContext.Session.SetString("State", state);//储存随机数到Session
            ViewData["returnUrl"] = returnUrl;
            ViewData["UrlUserInfo"] = OAuthApi.GetAuthorizeUrl(appId, callbackUrl, state, OAuthScope.snsapi_userinfo);
            return View();
        }


        /// <summary>
        /// OAuthScope.snsapi_userinfo方式回调 官方授权后的回调
        /// </summary>
        /// <param name="code">微信服务器带回来的</param>
        /// <param name="state">自己服务器生成的随机数做验证用</param>
        /// <param name="returnUrl">用户最初尝试进入的页面</param>
        /// <returns></returns>
        public async Task<ActionResult> UserInfoCallback(string code, string state, string returnUrl)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }
            // HttpContext.Session.SetString("State", state);
            //if (state != HttpContext.Session.GetString("State"))
            //{
            //    //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下，
            //    //建议用完之后就清空，将其一次性使用
            //    //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
            //    return Content("验证失败！请从正规途径进入！");
            //}
            OAuthAccessTokenResult result;
            try
            {
                // 通过，用code换取access_token
                result = await OAuthApi.GetAccessTokenAsync(appId, appSecret, code);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }

            //下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
            //如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
            //HttpContext.Session.SetString("OAuthAccessTokenStartTime", SystemTime.Now.ToString());
            //HttpContext.Session.SetString("OAuthAccessToken", result.ToJson());

            //因为第一步选择的是OAuthScope.snsapi_userinfo，这里可以进一步获取用户详细信息
            try
            {
                HttpContext.Session.SetString("OAuthAccessTokenResult", result.ToJson());
                //HttpContext.Session.SetString("OpenId", result.openid);
                return Redirect(returnUrl);

                //OAuthUserInfo userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
                //if (string.IsNullOrEmpty(returnUrl)) return View(userInfo);
                //HttpContext.Session.SetString("Userinfo", userInfo.ToJson());
                //return Redirect(returnUrl);
            }
            catch (ErrorJsonResultException ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult OAuthLogin()
        {
            var state = "Liuzhe-" + SystemTime.Now.Millisecond;//随机数，用于识别请求可靠性
            var callbackUrl = "http://wx.360wll.cn/OAuth2/UserInfoCallback?returnUrl=";
            HttpContext.Session.SetString("State", state);//储存随机数到Session

            var oauthOffcialUrl = OAuthApi.GetAuthorizeUrl(appId, callbackUrl, state, OAuthScope.snsapi_userinfo);
            return Redirect(oauthOffcialUrl);
        }
    }
}