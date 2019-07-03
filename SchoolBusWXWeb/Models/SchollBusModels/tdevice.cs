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
        public string pkid { get; set; }

        [StringLength(36)]
        public string fk_company_id { get; set; }

        [Required]
        [StringLength(20)]
        public string fcode { get; set; }

        public int fstate { get; set; }

        [Required(ErrorMessage = "±ÿ–ÎÃÓ–¥≥µ≈∆∫≈")]
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
