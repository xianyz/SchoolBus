using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    public class tsms
    {
        [Key]
        public string pkid { get; set; } = Guid.NewGuid().ToString("N");
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
        [BindNever]
        public DateTime fsendtime { get; set; }=DateTime.Now;
        /// <summary>
        /// 验证短信时间
        /// </summary>
        [BindNever]
        public DateTime finvalidtime { get; set; } = DateTime.Now;
        /// <summary>
        /// 状态 0:发送后未验证
        /// </summary>
        public int ftype { get;set;}
    }
}
