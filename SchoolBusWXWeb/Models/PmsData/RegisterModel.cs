using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace SchoolBusWXWeb.Models.PmsData
{
    public class RegisterModel
    {
        /// <summary>
        /// 微信ID
        /// </summary>
        [BindNever]
        public string wxid { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        [BindNever]
        public string userName { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        [Required(ErrorMessage = "请填写卡号")]
        [StringLength(50)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string cardNum { get; set; }
        /// <summary>
        /// 与学生关系
        /// </summary>
        [Required(ErrorMessage = "请填写与学生关系")]
        [StringLength(10)]
        //[RegularExpression(@"^[父亲][母亲][\u7237\u7237][奶奶][姥爷][姥姥][其他]", ErrorMessage = "和学生关系不正确")]
        [RegularExpression(@"^[爷爷]$", ErrorMessage = "和学生关系不正确")]
        public string relationship { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = "填写正确手机号码")]
        [StringLength(11, ErrorMessage = "手机号不能超过11个字符")]
        [RegularExpression(@"^1[3456789][0-9]{9}$", ErrorMessage = "手机号格式不正确")]
        public string phoneNum { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "填写正确验证码")]
        [StringLength(6, ErrorMessage = "手机号不能超过6个字符")]
        public string verificationCode { get; set; }
    }
}
