using Microsoft.AspNetCore.Mvc;
using SchoolBusWXWeb.Business;
using SchoolBusWXWeb.Models.PmsData;
using SchoolBusWXWeb.Models.ViewData;
using System.Threading.Tasks;

namespace SchoolBusWXWeb.Controllers
{
    public class SchoolBusController : ControllerEx // OAuthAndJsSdkController 
    {
        private readonly ISchoolBusBusines _schoolBusBusines;
        public SchoolBusController(ISchoolBusBusines schoolBusBusines)
        {
            _schoolBusBusines = schoolBusBusines;
        }

        #region 注册
        /// <summary>
        /// https://localhost:5001/schoolbus/Register
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var data = await _schoolBusBusines.GetTwxuserAsync("2c9ab45969dc19990169dd5bb9ea08b5");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel user)
        {
            var res = new RegisVD();
            if (ModelState.IsValid)
            {
#if DEBUG
                user.wxid = "oBcNx1lHzHxIpKm5m64XX99zTMGs";
                user.userName = "测试昵称";
#else
                //user.wxid=UserInfoe.openid;
#endif
                res = await _schoolBusBusines.DoRegisterAsync(user);
            }
            else
            {
                res.msg = GetModelStateError();
            }
            return Json(res);
        }
        #endregion

        #region 获取验证码
        [HttpPost]
        public async Task<IActionResult> SendSmsCode(SmsModel sms)
        {
            var smsVD = new SmsVD();
            if (ModelState.IsValid)
            {
                smsVD = await _schoolBusBusines.SendSmsCodeAsync(sms);
            }
            else
            {
                smsVD.msg = GetModelStateError();
            }
            return Json(smsVD);
        }
        #endregion

        #region 绑定成功提示页面
        [HttpGet]
        public IActionResult GoBinding()
        {
            return View();
        }
        #endregion
    }
}