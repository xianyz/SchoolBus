using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    [Table("public.tcardlog")]
    public class tcardlog
    {
        [Key]
        [StringLength(36)]
        public string pkid { get; set; }

        [Required]
        [StringLength(20)]
        public string fcode { get; set; }

        [StringLength(50)]
        public string fid { get; set; }

        public DateTime fcreatetime { get; set; }

        public decimal? flng { get; set; }

        public decimal? flat { get; set; }

        public int ftype { get; set; }
    }
}
