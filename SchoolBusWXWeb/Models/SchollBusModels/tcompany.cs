using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    [Table("public.tcompany")]
    public class tcompany
    {
        
        [Key]
        [StringLength(36)]
        public string pkid
        {
            get => string.IsNullOrEmpty(_pkid) ? Guid.NewGuid().ToString("N") : _pkid.TrimEnd();
            set => _pkid = !string.IsNullOrEmpty(value) ? value : Guid.NewGuid().ToString("N");
        }
        private string _pkid;

        [Required]
        [StringLength(36)]
        public string fk_region_id
        {
            get => _fk_region_id?.TrimEnd();
            set => _fk_region_id = value;
        }
        private string _fk_region_id;
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
