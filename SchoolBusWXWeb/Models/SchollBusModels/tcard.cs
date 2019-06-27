using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    public class tcard
    {
        [Key]
        public string pkid { get; set; } = Guid.NewGuid().ToString("N");
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
        /// 状态 目前0,1,2
        /// </summary>
        public int fstatus { get; set; }
        /// <summary>
        /// 设备id 外键 tdevice 表主键
        /// </summary>
        public string fk_device_id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [StringLength(50)]
        public string fname { get; set; }
        /// <summary>
        /// 性别 0:男 1:女
        /// </summary>
        public int fsex { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime fbirthdate { get; set; }
        /// <summary>
        /// 学校id 外键 tschool 表主键
        /// </summary>
        public string fk_school_id { get; set; }
        /// <summary>
        /// 上车地址
        /// </summary>
        [StringLength(10)]
        public string fboardingaddress { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string fremark { get; set; }
        /// <summary>
        /// 试用到期时间
        /// </summary>
        public DateTime? ftrialdate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime fcreatetime { get; set; }

        #region 其他表字段信息
        /// <summary>
        /// 车牌号
        /// </summary>
        [StringLength(10)]
        public string fplatenumber { get;set;}
        /// <summary>
        /// 学校名称
        /// </summary>
        [StringLength(50)]
        public string fschoolname { get;set;}
        #endregion
    }
}
