using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolBusWXWeb.Models.PmsData;
using SchoolBusWXWeb.Models.SchollBusModels;
using SchoolBusWXWeb.Models.ViewData;
using SchoolBusWXWeb.Repository;

namespace SchoolBusWXWeb.Business
{
    public class SchoolBusBusines : ISchoolBusBusines
    {
        private readonly ISchoolBusRepository _schoolBusRepository;
        public SchoolBusBusines(ISchoolBusRepository schoolBusRepository)
        {
            _schoolBusRepository = schoolBusRepository;
        }

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


        public async Task<RegisVD> DoRegisterAsync(RegisterModel user)
        {
            RegisVD regis = new RegisVD();
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
            DateTime st = DateTime.Now;
            DateTime et = st.AddMinutes(-10);
            var codeList = await _schoolBusRepository.GetSmsListBySendTimeAsync(user.phoneNum, 0, st, et);
            if (codeList.Count > 0)
            {
                var codeM = codeList.FirstOrDefault(c => c.fvcode == user.verificationCode);
                if (codeM == null)
                {
                    return new RegisVD { msg = "验证码错误，请重新输入" };
                }

                if (st > codeM.finvalidtime)
                {
                    return new RegisVD { msg = "验证码超时" };
                }
            }
            else
            {
                return new RegisVD { msg = "验证码超时" };
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
                    fstatus = 0
                };
                var res = await _schoolBusRepository.InsertWxUserAsync(wxuser);
                if (res > 0)
                {
                    regis.status = 1;
                    regis.msg = "注册成功";
                }
                else
                {
                    regis.msg = "注册失败,请稍后尝试";
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
                userRecord.fk_card_id = cardRecord.pkid;
                userRecord.frelationship = user.relationship;
                userRecord.fphone = user.phoneNum;
                var s2= await _schoolBusRepository.UpdateWxUserAsync(userRecord);
                if (s2 == 0)
                {
                    return new RegisVD { msg = "更新已有用户信息失败" };
                }
                #endregion
            }
            #endregion

            #region 卡片信息维护
            if (cardRecord.ftrialdate != null)
            {
                DateTime triald = Convert.ToDateTime(cardRecord.ftrialdate);
                var trialdateRecord = await _schoolBusRepository.GetSchoolConfigAsync("001"); // 首次注册试用期（天）
                int.TryParse(trialdateRecord.fvalue, out int tt);
                cardRecord.ftrialdate = triald.AddYears(tt);  // 卡片试用期赋值
            }
            // 维护卡片信息状态
            cardRecord.fstatus = 1;
            var s3= await _schoolBusRepository.UpdateTCardAsync(cardRecord);
            return s3 == 0 ? new RegisVD { msg = "维护卡片信息失败" } : regis;
            #endregion
        }
    }
}
