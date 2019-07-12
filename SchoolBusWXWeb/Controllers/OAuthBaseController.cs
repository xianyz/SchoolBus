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
using Microsoft.AspNetCore.Mvc;
using SchoolBusWXWeb.Business;

namespace SchoolBusWXWeb.Controllers
{
    [CustomOAuth(null, "/OAuth2/UserInfoCallback")]
    public class OAuthBaseController : Controller
    {
        protected OAuthAccessTokenResult TokenResult { get; private set; }
        private readonly ISchoolBusBusines _schoolBusBusines;
        public OAuthBaseController(ISchoolBusBusines schoolBusBusines)
        {
            _schoolBusBusines = schoolBusBusines;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // do something before the action executes
            TokenResult = JsonConvert.DeserializeObject<OAuthAccessTokenResult>(context.HttpContext.Session.GetString("OAuthAccessTokenResult"));
            var controller = context.RouteData.Values["controller"]?.ToString();
            var actionName = context.RouteData.Values["action"]?.ToString();
            if (controller?.ToUpper() == "SCHOOLBUS" && actionName?.ToUpper() != "REGISTER")
            {
                var result = await _schoolBusBusines.GetCardInfoByCodeAsync(TokenResult?.openid);
                if (result == null)
                {
                    context.Result = new RedirectToActionResult("Register", "SchoolBus", null);
                    return;
                }
            }
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

        #region 公共方法

        /// <summary>
        /// 获取服务端验证的第一条错误信息
        /// </summary>
        /// <returns></returns>
        public string GetModelStateError()
        {
            foreach (var item in ModelState.Values)
                if (item.Errors.Count > 0)
                    return item.Errors[0].ErrorMessage;
            return "";
        }

        /// <summary>
        ///  重定向验证
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public IActionResult RedirectToLocal(string returnUrl, string action = "Index", string controller = "Home")
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(action, controller);
        }

        #endregion 公共方法
    }
}
