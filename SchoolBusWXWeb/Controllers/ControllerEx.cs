using Microsoft.AspNetCore.Mvc;

namespace SchoolBusWXWeb.Controllers
{
    public class ControllerEx : Controller
    {
        #region 公共方法

        /// <summary>
        ///     获取服务端验证的第一条错误信息
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
        ///     重定向验证
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