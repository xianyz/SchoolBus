using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    public class twxuser
    {
        [Key]
        public string pkid { get; set; } = Guid.NewGuid().ToString("N");
        /// <summary>
        /// 微信openid
        /// </summary>
        [BindNever]
        public string fwxid { get; set; }
        /// <summary>
        /// 卡片id   外键 tcard 表fcode字段
        /// </summary>
        [BindNever]
        public string fk_card_id { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        [BindNever]
        public string fname { get; set; }
        /// <summary>
        /// 与学生关系
        /// </summary>
        [Required(ErrorMessage = "请填写与学生关系")]
        [StringLength(10)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string frelationship { get; set; } = string.Empty;
        /// <summary>
        /// 电话
        /// </summary>
        [Required(ErrorMessage = "填写正确手机号码")]
        [StringLength(11, ErrorMessage = "手机号不能超过11个字符")]
        [RegularExpression(@"^1[3456789][0-9]{9}$", ErrorMessage = "手机号格式不正确")]
        public string fphone { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [BindNever]
        public int fstatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [BindNever]
        public string fremark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [BindNever]
        public DateTime fcreatetime { get; set; } = DateTime.Now;
    }
}
