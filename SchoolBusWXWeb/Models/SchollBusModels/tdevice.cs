using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    [Table("public.tdevice")]
    public class tdevice
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
        /// 校车公司主键
        /// </summary>
        [StringLength(36)]
        public string fk_company_id
        {
            get => _fk_company_id?.TrimEnd();
            set => _fk_company_id = value;
        }
        private string _fk_company_id;

        [Required]
        [StringLength(20)]
        public string fcode { get; set; }

        public int fstate { get; set; }

        [Required(ErrorMessage = "必须填写车牌号")]
        [StringLength(10)]
        [RegularExpression(@"^(([\u4e00-\u9fa5][a-zA-Z]|[\u4e00-\u9fa5]{2}\d{2}|[\u4e00-\u9fa5]{2}[a-zA-Z])[-]?|([wW][Jj][\u4e00-\u9fa5]{1}[-]?)|([a-zA-Z]{2}))([A-Za-z0-9]{5}|[DdFf][A-HJ-NP-Za-hj-np-z0-9][0-9]{4}|[0-9]{5}[DdFf])$")]
        public string fplatenumber { get; set; }

        [StringLength(20)]
        public string fdriver { get; set; }

        [StringLength(20)]
        public string fdriverphone { get; set; }

        [StringLength(200)]
        public string fremark { get; set; }

        public DateTime fcreatetime { get; set; }
    }
}
