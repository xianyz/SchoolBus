using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    [Table("public.tlocatelog")]
    public class tlocatelog
    {
        [Key]
        [StringLength(36)]
        public string pkid { get; set; }

        [Required]
        [StringLength(20)]
        public string fcode { get; set; }

        public DateTime fcreatetime { get; set; }

        public decimal? flng { get; set; }

        public decimal? flat { get; set; }
    }
}
