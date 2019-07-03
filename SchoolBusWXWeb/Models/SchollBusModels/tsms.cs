using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    [Table("public.tsms")]
    public class tsms
    {
        private string _pkid;
        [Key]
        [StringLength(36)]
        public string pkid
        {
            get => string.IsNullOrEmpty(_pkid) ? Guid.NewGuid().ToString("N") : _pkid.TrimEnd();
            set => _pkid = !string.IsNullOrEmpty(value) ? value : Guid.NewGuid().ToString("N");
        }
        /// <summary>
        /// 电话
        /// </summary>
        [Required(ErrorMessage = "填写正确手机号码")]
        [StringLength(11, ErrorMessage = "手机号不能超过11个字符")]
        [RegularExpression(@"^1[3456789][0-9]{9}$", ErrorMessage = "手机号格式不正确")]
        public string fphone { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "填写正确验证码")]
        [StringLength(6, ErrorMessage = "手机号不能超过6个字符")]
        public string fvcode { get; set; }
        /// <summary>
        /// 发送短信时间
        /// </summary>
        public DateTime fsendtime { get; set; }
        /// <summary>
        /// 验证短信时间郭琦时间 是fsendtime+10分钟时间
        /// </summary>
        public DateTime finvalidtime { get; set; }
        /// <summary>
        /// 状态 0：注册，1：登录，2：修改
        /// </summary>
        public int ftype { get;set;}
    }
}
