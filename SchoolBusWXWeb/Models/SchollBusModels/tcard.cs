using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    [Table("public.tcard")]
    public class tcard
    {

        [Key]
        [StringLength(36)]
        public string pkid
        {
            get => string.IsNullOrEmpty(_pkid) ? Guid.NewGuid().ToString("N") : _pkid.TrimEnd();
            set => _pkid = !string.IsNullOrEmpty(value) ? value : Guid.NewGuid().ToString("N");
        }
        private string _pkid;

        /// <summary>
        /// 卡号
        /// </summary>
        [StringLength(50)]
        public string fid { get; set; }
        /// <summary>
        /// 卡号编码 (暂时和卡号一致)
        /// </summary>
        [StringLength(50)]
        public string fcode { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(20)]
        public string fpwd { get; set; }
        /// <summary>
        /// 状态 1:微信已经注册 2:挂失 3:注销 默认:0
        /// </summary>
        public int fstatus { get; set; }

        /// <summary>
        /// 设备id 外键 tdevice 表主键
        /// </summary>
        [StringLength(36)]
        public string fk_device_id
        {
            get => _fk_device_id?.TrimEnd();
            set => _fk_device_id = value;
        }
        private string _fk_device_id;

        /// <summary>
        /// 学生姓名
        /// </summary>
        [StringLength(50)]
        public string fname { get; set; }
        /// <summary>
        /// 性别 0:男 1:女 3:其他
        /// </summary>
        public int fsex { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? fbirthdate { get; set; }

        /// <summary>
        /// 学校id 外键 tschool 表主键
        /// </summary>
        [StringLength(36)]
        public string fk_school_id
        {
            get => _fk_school_id?.TrimEnd();
            set => _fk_school_id = value;
        }
        private string _fk_school_id;

        /// <summary>
        /// 上车地址
        /// </summary>
        [StringLength(10)]
        public string fboardingaddress
        {
            get => _fboardingaddress?.TrimEnd();
            set => _fboardingaddress = value;
        }
        private string _fboardingaddress = "";
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string fremark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime fcreatetime { get; set; }

        /// <summary>
        /// 试用到期时间
        /// </summary>
        public DateTime? ftrialdate { get; set; }


        #region 其他表字段信息
        /// <summary>
        /// 车牌号
        /// </summary>
        [StringLength(10)]
        public string fplatenumber { get; set; }
        /// <summary>
        /// 学校名称
        /// </summary>
        [StringLength(50)]
        public string fschoolname { get; set; }
        #endregion
    }
}
