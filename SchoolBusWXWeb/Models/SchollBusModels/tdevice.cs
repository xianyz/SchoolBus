using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    [Table("public.tdevice")]
    public  class tdevice
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

        [Required]
        [StringLength(10)]
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
