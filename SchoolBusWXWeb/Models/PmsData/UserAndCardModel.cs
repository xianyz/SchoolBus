using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolBusWXWeb.Models.PmsData
{
    /// <summary>
    /// 完善信息页面 展示数据模型
    /// </summary>
    public class UserAndCardModel
    {
        /// <summary>
        /// twxuser 表主键
        /// </summary>
        public string wxpkid { get; set; }

        /// <summary>
        /// 绑定卡人手机号(家长手机号)
        /// </summary>
        public string fphone { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string verificationCode { get; set; } = string.Empty;
        /// <summary>
        /// 与学生关系
        /// </summary>
        public string frelationship { get; set; }

        /// <summary>
        /// tcard表主键
        /// </summary>
        public string pkid { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        public string fcode { get; set; }

        /// <summary>
        /// 卡状态 1:微信已经注册 2:挂失 3:注销 默认:0
        /// </summary>
        public string fstatus { get; set; }

        /// <summary>
        /// 学生姓名
        /// </summary>
        public string fname { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime fbirthdate { get; set; }

        /// <summary>
        /// 上车地址
        /// </summary>
        public string fboardingaddress { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string fplatenumber { get; set; }

        /// <summary>
        /// 学校名称
        /// </summary>
        public string fschoolname { get; set; }

        /// <summary>
        /// 微信分享链接
        /// </summary>
        public string wxLink { get; set; }

        /// <summary>
        /// 微信分享图标
        /// </summary>
        public string wximgUrl { get; set; }
        /// <summary>
        /// 微信分享标题
        /// </summary>
        public string wxshareTitle { get; set; }

        /// <summary>
        /// 微信分享描述
        /// </summary>
        public string wxshareDescription { get; set; }
    }
}
