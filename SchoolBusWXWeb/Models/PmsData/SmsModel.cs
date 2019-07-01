using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolBusWXWeb.Models.PmsData
{
    public class SmsModel
    { 
        /// <summary>
        /// 电话
        /// </summary>
        [Required(ErrorMessage = "填写正确手机号码")]
        [StringLength(11, ErrorMessage = "手机号不能超过11个字符")]
        [RegularExpression(@"^1[3456789][0-9]{9}$", ErrorMessage = "手机号格式不正确")]
        public string phoneNum { get; set; }
        
        /// <summary>
        /// 状态 0：注册，1：登录，2：修改
        /// </summary>
        public int verificationCodeType { get; set; }
    }
}
