using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    [Table("public.twxpushlog")]
    public class twxpushlog
    {
        [Key]
        [StringLength(36)]
        public string pkid { get; set; }

        [Required]
        [StringLength(36)]
        public string fk_id { get; set; }

        [StringLength(50)]
        public string fwxid { get; set; }

        public DateTime fcreatetime { get; set; }

        public int fstate { get; set; }

        [StringLength(200)]
        public string fmsg { get; set; }
    }
}
