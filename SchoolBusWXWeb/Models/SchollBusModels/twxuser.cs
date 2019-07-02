using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    [Table("public.twxuser")]
    public class twxuser
    {
        private string _pkid;
        [Key]
        [StringLength(36)]
        public string pkid
        {
            get => _pkid.TrimEnd();
            set => _pkid = !string.IsNullOrEmpty(value) ? value : Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 微信openid
        /// </summary>
        [Required(ErrorMessage = "微信OpenId不能为空")]
        [StringLength(50)]
        public string fwxid { get; set; }

        private string _fk_card_id;
        /// <summary>
        /// 卡片id   外键 tcard 表pkid字段
        /// </summary>
        [Required(ErrorMessage = "卡片Id不能为空")]
        [StringLength(36)]
        public string fk_card_id
        {
            get => _fk_card_id.TrimEnd();
            set => _fk_card_id = value;
        }
        /// <summary>
        /// 微信昵称
        /// </summary>
        [Required(ErrorMessage = "微信昵称不能为空")]
        [StringLength(50)]
        public string fname { get; set; }
        /// <summary>
        /// 与学生关系
        /// </summary>
        [Required(ErrorMessage = "请填写与学生关系")]
        [StringLength(10)]
        public string frelationship { get; set; }
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
        public int fstatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string fremark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime fcreatetime { get; set; } = DateTime.Now;
    }
}
