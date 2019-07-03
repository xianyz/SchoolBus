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
        public string pkid
        {
            get => string.IsNullOrEmpty(_pkid) ? Guid.NewGuid().ToString("N") : _pkid.TrimEnd();
            set => _pkid = !string.IsNullOrEmpty(value) ? value : Guid.NewGuid().ToString("N");
        }
        private string _pkid;

        [Required]
        [StringLength(36)]
        public string fk_id
        {
            get => _fk_id?.TrimEnd();
            set => _fk_id = value;
        }
        private string _fk_id;
        [StringLength(50)]
        public string fwxid { get; set; }

        public DateTime fcreatetime { get; set; }

        public int fstate { get; set; }

        [StringLength(200)]
        public string fmsg { get; set; }
    }
}
