using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    [Table("public.tcompany_school")]
    public class tcompany_school
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
        public string fk_company_id
        {
            get => _fk_company_id?.TrimEnd();
            set => _fk_company_id = value;
        }
        private string _fk_company_id;
      
        [Required]
        [StringLength(36)]
        public string fk_school_id
        {
            get => _fk_school_id?.TrimEnd();
            set => _fk_school_id = value;
        }
        private string _fk_school_id;

        public DateTime fcreatetime { get; set; }
    }
}
