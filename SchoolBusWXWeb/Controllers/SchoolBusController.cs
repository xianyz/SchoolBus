using Microsoft.AspNetCore.Mvc;
using SchoolBusWXWeb.Business;
using SchoolBusWXWeb.Models.PmsData;
using SchoolBusWXWeb.Models.ViewData;
using System;
using System.Threading.Tasks;

#if !DEBUG
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
#endif

namespace SchoolBusWXWeb.Controllers
{
#if DEBUG
    public class SchoolBusController : ControllerEx
    {
        private const string Openid = "ovzSu1Ux_R10fGTWCEawfdVADSy800"; // oBcNx1lHzHxIpKm5m64XX99zTMGs // ovzSu1Ux_R10fGTWCEawfdVADSy8 
        private const string Nickname = "测试昵称";
        private readonly ISchoolBusBusines _schoolBusBusines;
        public SchoolBusController(ISchoolBusBusines schoolBusBusines) : base(schoolBusBusines, Openid)
        {
            _schoolBusBusines= schoolBusBusines;
        }

#else
    public class SchoolBusController : OAuthBaseController
    {
        private readonly ISchoolBusBusines _schoolBusBusines;
        public SchoolBusController(ISchoolBusBusines schoolBusBusines) : base(schoolBusBusines)
        {
            _schoolBusBusines = schoolBusBusines;
        }
#endif

        #region 默认页
        public async Task<IActionResult> Index(int type)
        {

#if DEBUG
            const string wxid = Openid;
#else
            string wxid = TokenResult.openid;
#endif
            ViewData["OpenId"] = wxid;
            var code = await _schoolBusBusines.GetUserCodeAsync(wxid);
            switch (type)
            {
                case 0 when (code == 1 || code == 4 || code == 5):
                    return RedirectToAction("Register");
                case 1 when (code == 2 || code == 3):
                    return RedirectToAction("GoReport");
                case 2 when code == 3:
                    return RedirectToAction("GoCardInfo");
                case 3 when code == 3:
                    return RedirectToAction("GoAddress", new { showType =1});
                case 4 when code == 3:
                    return RedirectToAction("GoCalendar");
                case 5 when code != 1:
                    return RedirectToAction("GoUntying");
            }

            return View(new IndexModel { type = type, code = code });

        }

        #endregion

        #region 注册
        /// <summary>
        /// https://localhost:5001/schoolbus/Register
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Register()
        {
#if DEBUG
            ViewData["OpenId"] = Openid;
#else
            ViewData["OpenId"] = TokenResult.openid;
#endif
            return View(new RegisterModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel user)
        {
            var res = new RegisVD();
            if (ModelState.IsValid)
            {
#if DEBUG
                user.wxid = Openid;
                user.userName = Nickname;
#else
                OAuthUserInfo userInfo = OAuthApi.GetUserInfo(TokenResult.access_token, TokenResult.openid);
                user.wxid = userInfo.openid;
                user.userName = userInfo.nickname;
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
            var smsVd = new SmsVD();
            if (ModelState.IsValid)
            {
                smsVd = await _schoolBusBusines.SendSmsCodeAsync(sms);
            }
            else
            {
                smsVd.msg = GetModelStateError();
            }
            return Json(smsVd);
        }
        #endregion

        #region 绑定成功提示页面
        [HttpGet]
        public IActionResult GoBinding()
        {
            return View(new Base_JSSDKVD());
        }
        #endregion

        #region 完善信息并保存
        [HttpGet]
        public async Task<IActionResult> GoCardInfo()
        {
#if DEBUG
            const string wxid = Openid;
#else
            var wxid = TokenResult.openid;
#endif
            ViewData["OpenId"] = wxid;
            var result = await _schoolBusBusines.GetCardInfoByCodeAsync(wxid);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCardInfo(UserAndCardModel userAndCard)
        {
            var res = new SaveCardInfoVD();
            if (ModelState.IsValid)
            {
#if DEBUG
                userAndCard.wxid = Openid;
#else
                userAndCard.wxid = TokenResult.openid;
#endif
                res = await _schoolBusBusines.SavaCardAndUserInfoAsync(userAndCard);
            }
            else
            {
                res.msg = GetModelStateError();
            }
            return Json(res);
        }
        #endregion

        #region 解绑
        [HttpGet]
        public IActionResult GoUntying()
        {
            return View(new Base_JSSDKVD());
        }

        [HttpPost]
        public async Task<IActionResult> DoUntying()
        {
#if DEBUG
            const string wxid = Openid;
#else
            string wxid = TokenResult.openid;
#endif
            var result = await _schoolBusBusines.UntringAsync(wxid);
            return Json(result);
        }
        #endregion

        #region 挂失
        [HttpGet]
        public IActionResult GoReport()
        {
            return View(new Base_JSSDKVD());
        }

        [HttpPost]
        public async Task<IActionResult> DoReport()
        {
#if DEBUG
            const string wxid = Openid;
#else
            string wxid = TokenResult.openid;
#endif
            var result = await _schoolBusBusines.UnReportAsync(wxid);
            return Json(result);
        }
        #endregion

        #region 根据车牌号获取托运的学校 "辽A00002"
        [HttpGet]
        public async Task<IActionResult> GetSchoolListByNum(string platenumber)
        {
            var result = await _schoolBusBusines.GetSchoolListByPlatenumberAsync(platenumber);
            return Json(result);
        }
        #endregion

        #region 校车位置
        [HttpGet]
        public async Task<IActionResult> GoAddress(int showType, string cardLogId = "")
        {

#if DEBUG
            const string wxid = Openid;
#else
            string wxid = TokenResult.openid;
#endif
            var data = await _schoolBusBusines.GetUserCardInfoAsync(wxid, showType, cardLogId);
            return View(data);
        }


        #endregion

        #region 刷卡日历
        [HttpGet]
        public async Task<IActionResult> GoCalendar()
        {
#if DEBUG
            const string wxid = Openid;
#else
            string wxid = TokenResult.openid;
#endif
            var data = await _schoolBusBusines.GetCardNumAsync(wxid);
            if (string.IsNullOrEmpty(data))
            {
                return RedirectToAction(actionName: "Register", "SchoolBus");
            }
            return View(new Base_JSSDKVD());
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendanceInfo(DateTime dt)
        {
#if DEBUG
            const string wxid = Openid;
#else
            string wxid = TokenResult.openid;
#endif
            var result = await _schoolBusBusines.GetAttendanceInfoAsync(wxid, dt);
            return Json(result);
        }
        #endregion

      
    }
}