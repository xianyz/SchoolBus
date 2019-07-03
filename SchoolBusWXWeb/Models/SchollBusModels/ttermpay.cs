using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    [Table("public.ttermpay")]
    public class ttermpay
    {
        [Key]
        [StringLength(36)]
        public string pkid { get; set; }

        [Required]
        [StringLength(36)]
        public string fk_term_id { get; set; }

        [StringLength(50)]
        public string fcode { get; set; }

        public DateTime fcreatetime { get; set; }
    }
}
