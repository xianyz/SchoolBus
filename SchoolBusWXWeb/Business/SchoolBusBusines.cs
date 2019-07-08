using Microsoft.Extensions.Options;
using SchoolBusWXWeb.Models;
using SchoolBusWXWeb.Models.PmsData;
using SchoolBusWXWeb.Models.SchollBusModels;
using SchoolBusWXWeb.Models.ViewData;
using SchoolBusWXWeb.Repository;
#if !DEBUG
using SchoolBusWXWeb.Utilities;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


// ReSharper disable SwitchStatementMissingSomeCases

namespace SchoolBusWXWeb.Business
{
    public class SchoolBusBusines : ISchoolBusBusines
    {
        private readonly SiteConfig _option;
        private readonly ISchoolBusRepository _schoolBusRepository;
        public SchoolBusBusines(ISchoolBusRepository schoolBusRepository, IOptions<SiteConfig> option)
        {
            _option = option.Value;
            _schoolBusRepository = schoolBusRepository;
        }

        /// <summary>
        /// TODO 根据主键获取用户信息
        /// </summary>
        /// <param name="pkid"></param>
        /// <returns></returns>
        public async Task<twxuser> GetTwxuserAsync(string pkid)
        {
            try
            {
                return await _schoolBusRepository.GetTwxuserBypkidAsync(pkid);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// 根据openid返回用户状态
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public async Task<int> GetUserCodeAsync(string wxid)
        {
            var userRecord = await _schoolBusRepository.GetTwxuserBytOpenidAsync(wxid);
            if (userRecord == null)
            {
                return 1; //该微信没注册
            }
            var cardRecord = await _schoolBusRepository.GetCardBypkidAsync(userRecord.fk_card_id); // 用户绑定卡片信息
            if (cardRecord == null)
            {
                return -1; //该微信所注册绑定的卡片已被删除
            }
            // 4:已注册并卡已挂失，跳转注册页重新绑定
            // 5:已注册并卡已注销，跳转注册页重新绑定
            if (cardRecord.fstatus != 1) return cardRecord.fstatus == 2 ? 4 : 5;

            // 验证试用期是否到期
            if (cardRecord.ftrialdate == null) return 3; //该微信已注册绑卡并且已成功绑定
            var triadate = Convert.ToDateTime(cardRecord.ftrialdate);
            var nowtdate = DateTime.Now;
            // 学期缴费信息
            if (string.IsNullOrEmpty(cardRecord.fcode) && triadate < nowtdate)
            {
                return 2; // 该微信已注册绑卡并且卡已到期
            }
            var termPayRecord = await _schoolBusRepository.GetTermPayRecordByFCodeAsync(cardRecord.fcode);
            if (termPayRecord != null && triadate < nowtdate && termPayRecord.fenddate < nowtdate)
            {
                return 2;// 该微信已注册绑卡并且卡已到期
            }
            return 3; //该微信已注册绑卡并且已成功绑定
        }

        /// <summary>
        /// TODO 用户注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<RegisVD> DoRegisterAsync(RegisterModel user)
        {
            #region 卡号校验
            var cardRecord = await _schoolBusRepository.GetCardByCodeAsync(user.cardNum);
            if (cardRecord == null)
            {
                return new RegisVD { msg = "卡号错误，请重新输入" };
            }
            switch (cardRecord.fstatus)
            {
                case 2:
                    return new RegisVD { msg = "该卡已挂失，请重新输入" };
                case 3:
                    return new RegisVD { msg = "该卡已注销，请重新输入" };
            }
            #endregion

            #region 验证码校验
            var isCode = await CheckCode(user.phoneNum, user.verificationCode, 0);
            if (isCode.status != 1)
            {
                return new RegisVD { msg = isCode.msg };
            }
            #endregion

            #region 微信号校验和注册操作
            var userRecord = await _schoolBusRepository.GetTwxuserBytOpenidAsync(user.wxid);
            if (userRecord == null) // 用户没有注册过
            {
                twxuser wxuser = new twxuser
                {
                    fwxid = user.wxid,
                    fname = user.userName,
                    fk_card_id = cardRecord.pkid,
                    frelationship = user.relationship,
                    fphone = user.phoneNum,
                    fstate = 0
                };
                var res = await _schoolBusRepository.InsertWxUserAsync(wxuser);
                if (res == 0)
                {
                    return new RegisVD { msg = "注册失败,请稍后尝试" };
                }
            }
            else
            {
                // 根据当前用户微信获取之前绑卡卡号信息
                var userCardRecord = await _schoolBusRepository.GetCardBypkidAsync(userRecord.fk_card_id);// 这一步一定有卡信息
                if (userCardRecord == null)
                {
                    return new RegisVD { msg = "你注册的卡已经不存在,请联系管理员" };
                }
                if (userCardRecord.fstatus == 1)
                {
                    return new RegisVD { msg = "该微信已注册" };
                }

                // 老卡数据要导入新卡中
                if (!string.IsNullOrEmpty(cardRecord.fname) && !string.IsNullOrEmpty(userCardRecord.fname))
                {
                    cardRecord.fname = userCardRecord.fname;
                    cardRecord.fsex = userCardRecord.fsex;
                    cardRecord.fk_school_id = userCardRecord.fk_school_id;
                    cardRecord.fk_device_id = userCardRecord.fk_device_id;
                    cardRecord.fboardingaddress = userCardRecord.fboardingaddress;
                    cardRecord.fbirthdate = userCardRecord.fbirthdate;
                }
                // userCardRecord.pkid 之前卡片信息
                // cardRecord.pkid  新卡信息 
                // 更新所有绑定老卡用户卡片信息
                var s1 = await _schoolBusRepository.UpdateUserCardAsync(userCardRecord.pkid, cardRecord.pkid);
                if (s1 == 0)
                {
                    return new RegisVD { msg = "更新所有绑定老卡用户卡片信息" };
                }
                #region 更新已有用户信息
                userRecord.pkid = userRecord.pkid;
                userRecord.fk_card_id = cardRecord.pkid;
                userRecord.frelationship = user.relationship;
                userRecord.fphone = user.phoneNum;
                var s2 = await _schoolBusRepository.UpdateWxUserAsync(userRecord);
                if (s2 == 0)
                {
                    return new RegisVD { msg = "更新已有用户信息失败" };
                }
                #endregion
            }
            #endregion

            #region 卡片信息维护
            if (cardRecord.ftrialdate == null)
            {
                DateTime triald = Convert.ToDateTime(cardRecord.ftrialdate);
                var trialdateRecord = await _schoolBusRepository.GetSchoolConfigAsync("001"); // 首次注册试用期（天）
                int.TryParse(trialdateRecord.fvalue, out int tt);
                cardRecord.ftrialdate = triald.AddYears(tt);  // 卡片试用期赋值
            }
            cardRecord.fstatus = 1;   // 维护卡片信息状态 
            var s3 = await _schoolBusRepository.UpdateTCardAsync(cardRecord);
            #endregion
            return s3 == 0 ? new RegisVD { msg = "维护卡片信息失败" } : new RegisVD { status = 1, msg = "注册成功" };
        }

        /// <summary>
        /// TODO 完善用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SaveCardInfoVD> SavaCardAndUserInfoAsync(UserAndCardModel model)
        {
            var userRecord = await _schoolBusRepository.GetTwxuserBytOpenidAsync(model.wxid);
            if (userRecord != null)
            {
                #region 验证是否更换手机号
                if (string.IsNullOrEmpty(model.verificationCode))
                {
                    if (userRecord.fphone != model.fphone)
                    {
                        return new SaveCardInfoVD { msg = "请填写验证码" };
                    }
                }
                else
                {
                    var isCode = await CheckCode(model.fphone, model.verificationCode, 2);
                    if (isCode.status != 1)
                    {
                        return new SaveCardInfoVD { msg = isCode.msg };
                    }
                }

                #endregion

                #region 车牌号校验
                var deviceRecord = await _schoolBusRepository.GetDeviceByPlatenumberAsync(model.fplatenumber);
                if (deviceRecord == null)
                {
                    return new SaveCardInfoVD { msg = "该车未绑定设备" };
                }
                #endregion

                #region 校验车牌号和所在学校是否属于同一校车公司
                var schoolRecord = await _schoolBusRepository.GetSchoolByNameAsync(model.fschoolname);
                if (schoolRecord == null)
                {
                    return new SaveCardInfoVD { msg = "该学校不存在" };
                }
                var companySchoolRecord = await _schoolBusRepository.GetCompanySchoolRelAsync(deviceRecord.fk_company_id, schoolRecord.pkid);
                if (companySchoolRecord == null)
                {
                    return new SaveCardInfoVD { msg = "所输车牌号和学校不属于同一校车公司" };
                }
                #endregion

                #region 更新完善用户和卡信息
                // TODO 维护卡片信息
                var cardRecord = await _schoolBusRepository.GetCardBypkidAsync(userRecord.fk_card_id);
                if (cardRecord == null)
                {
                    return new SaveCardInfoVD { msg = "不存在此卡,请联系管理员" };
                }
                cardRecord.fname = model.fname;
                cardRecord.fk_school_id = schoolRecord.pkid;
                cardRecord.fk_device_id = deviceRecord.pkid;
                cardRecord.fboardingaddress = model.fboardingaddress.Trim();
                cardRecord.fbirthdate = model.fbirthdate;
                var iscard = await _schoolBusRepository.UpdateTCardAsync(cardRecord);
                if (iscard == 0)
                {
                    return new SaveCardInfoVD { msg = "维护卡片信息失败" };
                }
                // TODO 维护用户信息
                userRecord.fphone = model.fphone;
                userRecord.frelationship = model.frelationship;
                var isuser = await _schoolBusRepository.UpdateWxUserAsync(userRecord);
                if (isuser == 0)
                {
                    return new SaveCardInfoVD { msg = "维护用户信息失败" };
                }
                #endregion
            }
            else
            {
                return new SaveCardInfoVD { status = 2, msg = "当前用户还没有注册,请先注册后再完善信息" };
            }
            return new SaveCardInfoVD { status = 1, msg = "保存成功" };
        }

        /// <summary>
        /// TODO 发送短信验证码
        /// </summary>
        /// <param name="sms"></param>
        /// <returns></returns>
        public async Task<SmsVD> SendSmsCodeAsync(SmsModel sms)
        {
            DateTime date = DateTime.Now;
            DateTime beforedate = date.AddMinutes(-10);
            DateTime before1Mdate = date.AddMinutes(-1);
            DateTime invaliddate = date.AddMinutes(10); // 失效时间为10分钟
            // 获取十分钟内已发送验证码列表
            var codeList = await _schoolBusRepository.GetSmsListBySendTimeAsync(sms.phoneNum, sms.verificationCodeType, beforedate, date);
            // 验证发送次数
            if (codeList.Count >= 5)
            {
                return new SmsVD { msg = "短时间发送短信过多" };
            }
            // 发送间隔1分钟判断
            if (codeList.Any() && codeList.First().fsendtime > before1Mdate)
            {
                return new SmsVD { msg = "验证码已失效" };
            }

            // 生成6位随机数验证码
            Random ran = new Random();
            string code = ran.Next(100000, 999999).ToString();
#if DEBUG
            var smsresult = new AliSmsModel { Code = "OK" };
#else
            var smsresult = Tools.SendSms(sms.phoneNum, code);
#endif

            switch (smsresult.Code)
            {
                case "scfaile":
                    return new SmsVD { msg = smsresult.Message };
                case "OK":
                    {
                        tsms tsms = new tsms
                        {
                            fphone = sms.phoneNum,
                            fvcode = code,
                            fsendtime = date,
                            finvalidtime = invaliddate,
                            ftype = sms.verificationCodeType
                        };
                        await _schoolBusRepository.InsertSmsCodeAsync(tsms);
                        return new SmsVD { status = 1, msg = "发送成功" };
                    }
                default:
                    return new SmsVD { msg = "发送失败" };
            }

        }

        /// <summary>
        /// TODO 获取用户和卡信息
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public async Task<UserAndCardModel> GetCardInfoByCodeAsync(string wxid)
        {
            var data = await _schoolBusRepository.GetUserAndCardByOpenidAsync(wxid);
            if (data == null) return new UserAndCardModel();
            var configList = await _schoolBusRepository.GetSchoolConfigListAsync("'002','003'");
            data.wxshareTitle = configList.FirstOrDefault(x => x.fcode == "002")?.fvalue;
            data.wxshareDescription = configList.FirstOrDefault(x => x.fcode == "003")?.fvalue;
            data.wxLink = _option.WxShareOption.URL + "/index?type=0&cardNum=" + data.fcode;
            data.wximgUrl = _option.WxShareOption.URL + "/img/pic1.jpg";
            return data;
        }

        /// <summary>
        /// TODO 根据车牌号获取托运的学校
        /// </summary>
        /// <param name="platenumber"></param>
        /// <returns></returns>
        public async Task<SchoolVD> GetSchoolListByPlatenumberAsync(string platenumber)
        {
            var result = await _schoolBusRepository.GetSchoolListByPlatenumberAsync(platenumber);
            var schoolModes = new List<SchoolMode>();
            result.Select(p => new { p.ftype }).Distinct().ToList().ForEach(x =>
            {
                var schoolMode = new SchoolMode { value = x.ftype.ToString() };
                var typeList = new List<SchoolValueText>();
                result.Where(y => y.ftype == x.ftype).ToList().ForEach(z =>
                {
                    typeList.Add(new SchoolValueText
                    {
                        text = z.text,
                        value = z.value
                    });
                });
                switch (x.ftype)
                {
                    case 1:
                        schoolMode.text = "小学";
                        break;
                    case 2:
                        schoolMode.text = "中学";
                        break;
                    case 3:
                        schoolMode.text = "高中";
                        break;
                }
                schoolMode.children = typeList;
                schoolModes.Add(schoolMode);
            });
            var svd = new SchoolVD()
            {
                status = 1,
                msg = "成功",
                data = schoolModes
            };
            return svd;
        }

        /// <summary>
        /// TODO 解绑
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public async Task<BaseVD> UntringAsync(string wxid)
        {
            var userRecord = await _schoolBusRepository.GetTwxuserBytOpenidAsync(wxid);
            if (userRecord == null) return new BaseVD { status = 1, msg = "解绑成功" };
            var userCardRecord = await _schoolBusRepository.GetOtherUserByCardIdAsync(userRecord.fk_card_id, userRecord.pkid);
            if (userCardRecord == null) // 如果该卡下没有绑定微信用户
            {
                var cardRecord = await _schoolBusRepository.GetCardBypkidAsync(userRecord.fk_card_id);
                if (cardRecord != null)
                {
                    cardRecord.fname = null;
                    cardRecord.fsex = 3;
                    cardRecord.fstatus = 0;
                    cardRecord.fk_school_id = null;
                    cardRecord.fk_device_id = null;
                    cardRecord.fboardingaddress = "";
                    cardRecord.fbirthdate = null;
                    await _schoolBusRepository.UpdateTCardAsync(cardRecord); // 清空卡片信息
                }
            }
            // 删除当前绑定卡微信用户
            await _schoolBusRepository.DeleteWxUserAsync(userRecord.pkid);
            return new BaseVD { status = 1, msg = "解绑成功" };
        }
        
        /// <summary>
        /// 挂失
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public async Task<BaseVD> UnReportAsync(string wxid)
        {
            var userRecord = await _schoolBusRepository.GetTwxuserBytOpenidAsync(wxid);
            if (userRecord == null) return new BaseVD { status = 1, msg = "挂失成功" };
            var b = await _schoolBusRepository.UpdateCardStatusAsync(userRecord.fk_card_id, 2);
            return b > 0 ? new BaseVD { status = 1, msg = "挂失成功" } : new BaseVD { msg = "挂失失败" };
        }

        /// <summary>
        /// 获取用户卡信息
        /// </summary>
        /// <param name="wxid">微信openid</param>
        /// <param name="showType">0:刷卡位置 1:实时位置</param>
        /// <param name="cardLogId">刷卡位置 传入</param>
        /// <returns></returns>
        public async Task<AddressModel> GetUserCardInfoAsync(string wxid, int showType, string cardLogId = "")
        {
            var model = new AddressModel();
            var usercard = await _schoolBusRepository.GetUserCardInfoAsync(wxid); // 根据openId查询已导入绑定卡片信息
            if (usercard == null) return model;
            switch (usercard.fstatus) // 校验卡片状态
            {
                case 2:
                    model.status = 0;
                    break;
                case 3:
                    model.status = 1;
                    break;
            }

            if (!string.IsNullOrEmpty(usercard.fk_device_id))
            {
                #region 实时位置 OR 刷卡位置
                model.showType = showType;
                if (showType == 0)  // 刷卡位置
                {
                    var positionInfo = await _schoolBusRepository.GetCardLogBypkidAsync(cardLogId) ?? await _schoolBusRepository.GetLastCardLogAsync(usercard.fcode);

                    if (positionInfo == null)
                    {
                        model.status = 4;
                    }
                    else
                    {
                        model.fcode = positionInfo.fcode;
                        model.flng = positionInfo.flng;
                        model.flat = positionInfo.flat;
                        model.cardLogId = positionInfo.pkid;
                    }
                }
                else  // 实时位置
                {
                    var deviceRecord = await _schoolBusRepository.GetDeviceByPkidAsync(usercard.fk_device_id);
                    var positionInfo = await _schoolBusRepository.GetLastLocateLogAsync(deviceRecord.fcode);
                    if (positionInfo == null)
                    {
                        model.status = 3;
                    }
                    else
                    {
                        model.fcode = positionInfo.fcode;
                        model.flng = positionInfo.flng;
                        model.flat = positionInfo.flat;
                    }
                }
                #endregion

                model.student = usercard.student;
            }
            else
            {
                model.status = 2;
            }
            model.wxshareTitle = showType == 0 ? "刷卡位置" : "实时位置";
            model.wxshareDescription = "点击查看";
            model.wxLink = showType == 0 ?
                _option.WxShareOption.URL + "/SchoolBus/GoAddress?showType=" + showType + "&cardLogId=" + model.cardLogId :
                _option.WxShareOption.URL + "/SchoolBus/GoAddress?showType=" + showType;
            model.wximgUrl = _option.WxShareOption.URL + "/img/pic1.jpg";
            return model;
        }

        /// <summary>
        /// TODO 验证手机号和验证码
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="verificationCode"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private async Task<BaseVD> CheckCode(string phoneNum, string verificationCode, int type)
        {
            DateTime date = DateTime.Now;
            DateTime beforedate = date.AddMinutes(-10);
            var codeList = await _schoolBusRepository.GetSmsListBySendTimeAsync(phoneNum, type, beforedate, date);
            if (codeList.Count > 0)
            {
                var codeM = codeList.FirstOrDefault(c => c.fvcode == verificationCode);
                if (codeM == null)
                {
                    return new BaseVD { msg = "验证码错误，请重新输入" };
                }
                if (date > codeM.finvalidtime)
                {
                    return new BaseVD { msg = "验证码超时" };
                }
            }
            else
            {
                return new BaseVD { msg = "验证码超时" };
            }
            return new BaseVD { status = 1, msg = "ok" };
        }

    }

}
