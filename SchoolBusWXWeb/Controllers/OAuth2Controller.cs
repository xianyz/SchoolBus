using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Extensions;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Threading.Tasks;

// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Controllers
{
    public class OAuth2Controller : Controller
    {
        private readonly string appId = Config.SenparcWeixinSetting.WeixinAppId;
        private readonly string appSecret = Config.SenparcWeixinSetting.WeixinAppSecret;

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
            OAuthAccessTokenResult result;
            try
            {
                result = await OAuthApi.GetAccessTokenAsync(appId, appSecret, code); // 通过，用code换取access_token
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }

            try
            {
                HttpContext.Session.SetString("OAuthAccessTokenResult", result.ToJson());
                return Redirect(returnUrl);
            }
            catch (ErrorJsonResultException ex)
            {
                return Content(ex.Message);
            }
        }

    }
}