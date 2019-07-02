using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    [Table("public.tregion")]
    public  class tregion
    {
        [Key]
        [StringLength(36)]
        public string pkid { get; set; }

        [Required]
        [StringLength(20)]
        public string fcode { get; set; }

        [StringLength(20)]
        public string fpcode { get; set; }

        [Required]
        [StringLength(50)]
        public string fname { get; set; }

        [StringLength(200)]
        public string fremark { get; set; }

        public DateTime fcreatetime { get; set; }
    }
}
