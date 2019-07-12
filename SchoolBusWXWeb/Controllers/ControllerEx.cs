using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolBusWXWeb.Business;
using System.Threading.Tasks;

namespace SchoolBusWXWeb.Controllers
{
    public class ControllerEx : Controller
    {
        private readonly string _openid;
        private readonly ISchoolBusBusines _schoolBusBusines;
        public ControllerEx(ISchoolBusBusines schoolBusBusines, string openid)
        {
            _openid = openid;
            _schoolBusBusines = schoolBusBusines;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.RouteData.Values["controller"]?.ToString();
            var actionName = context.RouteData.Values["action"]?.ToString();
            if (controller?.ToUpper() == "SCHOOLBUS" && (actionName?.ToUpper() != "REGISTER" && actionName?.ToUpper() != "SENDSMSCODE"))
            {
                var result = await _schoolBusBusines.GetCardInfoByCodeAsync(_openid);
                if (result == null || string.IsNullOrEmpty(result.fcode) || string.IsNullOrEmpty(result.wxpkid))
                {
                    context.Result = new RedirectToActionResult("Register", "SchoolBus", null);// RedirectResult("~/Home/Index")
                    return;
                }
            }
            await next();
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