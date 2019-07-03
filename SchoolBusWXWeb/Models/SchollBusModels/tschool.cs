using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    [Table("public.tschool")]
    public class tschool
    {
        [Key]
        [StringLength(36)]
        public string pkid { get; set; }

        [Required]
        [StringLength(36)]
        public string fk_region_id { get; set; }

        [Required]
        [StringLength(50)]
        public string fname { get; set; }

        public int? ftype { get; set; }

        [StringLength(20)]
        public string ftel { get; set; }

        [StringLength(100)]
        public string faddress { get; set; }

        public decimal? flng { get; set; }

        public decimal? flat { get; set; }

        [StringLength(200)]
        public string fremark { get; set; }

        public DateTime fcreatetime { get; set; }
    }
}
