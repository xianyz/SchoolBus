using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    [Table("public.tcompany_school")]
    public class tcompany_school
    {
        [Key]
        [StringLength(36)]
        public string pkid { get; set; }

        [Required]
        [StringLength(36)]
        public string fk_company_id { get; set; }

        [Required]
        [StringLength(36)]
        public string fk_school_id { get; set; }

        public DateTime fcreatetime { get; set; }
    }
}
