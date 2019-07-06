using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    [Table("public.tlocatelog")]
    public class tlocatelog
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
        /// public.tdevice 表的 fcode 设备编码
        /// </summary>
        [Required]
        [StringLength(20)]
        public string fcode { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime fcreatetime { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public decimal? flng { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public decimal? flat { get; set; }
    }
}
