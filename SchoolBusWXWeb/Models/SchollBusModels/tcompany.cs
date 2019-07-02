using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    [Table("public.tcompany")]
    public class tcompany
    {
        [Key]
        [StringLength(36)]
        public string pkid { get; set; }

        [Required]
        [StringLength(36)]
        public string fk_region_id { get; set; }

        [Required]
        [StringLength(20)]
        public string fcode { get; set; }

        [Required]
        [StringLength(50)]
        public string fname { get; set; }

        public int fstate { get; set; }

        [StringLength(20)]
        public string fperson { get; set; }

        [StringLength(20)]
        public string ftel { get; set; }

        [StringLength(200)]
        public string faddress { get; set; }

        [StringLength(200)]
        public string fremark { get; set; }

        public DateTime fcreatetime { get; set; }
    }
}
