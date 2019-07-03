using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    [Table("public.tconfig")]
    public class tconfig
    {
       
        [Key]
        [StringLength(36)]
        public string pkid
        {
            get => string.IsNullOrEmpty(_pkid) ? Guid.NewGuid().ToString("N") : _pkid.TrimEnd();
            set => _pkid = !string.IsNullOrEmpty(value) ? value : Guid.NewGuid().ToString("N");
        }
        private string _pkid;

        /// <summary>
        /// 编码 
        /// </summary>
        [Required]
        [StringLength(20)]
        public string fcode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string fname { get; set; }
        /// <summary>
        /// 值 fcode=001 代表试用期是几天 002 003 是微信分享相关
        /// </summary>
        [StringLength(200)]
        public string fvalue { get;set;}
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string fremark { get;set;}
    }
}
