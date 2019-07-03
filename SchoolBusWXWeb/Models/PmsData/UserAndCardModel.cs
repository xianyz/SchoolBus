using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SchoolBusWXWeb.Models.PmsData
{
    /// <summary>
    /// 完善信息model
    /// </summary>
    public class UserAndCardModel
    {
        /// <summary>
        /// twxuser 表主键
        /// </summary>
        [BindNever]
        public string wxpkid { get; set; }
        /// <summary>
        /// wxopen id
        /// </summary>
        [BindNever]
        public string wxid { get; set; }
        /// <summary>
        /// 绑定卡人手机号(家长手机号)
        /// </summary>
        [Required(ErrorMessage = "填写正确手机号码")]
        [StringLength(11, ErrorMessage = "手机号不能超过11个字符")]
        [RegularExpression(@"^1[3456789][0-9]{9}$", ErrorMessage = "手机号格式不正确")]
        public string fphone { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        //[Required(ErrorMessage = "填写正确验证码")]
        [StringLength(6, ErrorMessage = "验证码不能超过6个字符")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string verificationCode { get; set; }

        /// <summary>
        /// 与学生关系
        /// </summary>
        [Required(ErrorMessage = "请填写与学生关系")]
        [StringLength(10)]
        [RegularExpression(@"^(父亲|母亲|爷爷|奶奶|姥爷|姥姥|其他)$", ErrorMessage = "和学生关系不正确")]
        public string frelationship { get; set; }

        /// <summary>
        /// tcard表主键
        /// </summary>
        [BindNever]
        public string pkid { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        [Required(ErrorMessage = "请填写卡号")]
        [StringLength(50)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string fcode { get; set; }

        /// <summary>
        /// 卡状态 1:微信已经注册 2:挂失 3:注销 默认:0
        /// </summary>
        [BindNever]
        public int fstatus { get; set; }

        /// <summary>
        /// 学生姓名
        /// </summary>
        [Required(ErrorMessage = "请填学生姓名")]
        [StringLength(50)]
        public string fname { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime fbirthdate { get; set; }=DateTime.Now.Date;

        /// <summary>
        /// 上车地址
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string fboardingaddress
        {
            get => _fboardingaddress?.TrimEnd();
            set => _fboardingaddress = value;
        }
        private string _fboardingaddress="";
        /// <summary>
        /// 车牌号
        /// </summary>
        [Required(ErrorMessage = "必须填写车牌号")]
        [StringLength(10)]
        [RegularExpression(@"^(([\u4e00-\u9fa5][a-zA-Z]|[\u4e00-\u9fa5]{2}\d{2}|[\u4e00-\u9fa5]{2}[a-zA-Z])[-]?|([wW][Jj][\u4e00-\u9fa5]{1}[-]?)|([a-zA-Z]{2}))([A-Za-z0-9]{5}|[DdFf][A-HJ-NP-Za-hj-np-z0-9][0-9]{4}|[0-9]{5}[DdFf])$")]
        public string fplatenumber { get; set; }

        /// <summary>
        /// 学校名称
        /// </summary>
        [Required(ErrorMessage = "必须填写学校")]
        [StringLength(50)]
        public string fschoolname { get; set; }

        /// <summary>
        /// 微信分享链接
        /// </summary>
        [BindNever]
        public string wxLink { get; set; }

        /// <summary>
        /// 微信分享图标
        /// </summary>
        [BindNever]
        public string wximgUrl { get; set; }

        /// <summary>
        /// 微信分享标题
        /// </summary>
        [BindNever]
        public string wxshareTitle { get; set; }

        /// <summary>
        /// 微信分享描述
        /// </summary>
        [BindNever]
        public string wxshareDescription { get; set; }

    }
}
