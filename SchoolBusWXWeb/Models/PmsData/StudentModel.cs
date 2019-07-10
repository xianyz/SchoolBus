// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace SchoolBusWXWeb.Models.PmsData
{
    public class StudentModel
    {
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string student { get; set; }

        /// <summary>
        /// 设备号
        /// </summary>
        public string fk_device_id
        {
            get => _fk_device_id?.TrimEnd();
            set => _fk_device_id = value;
        }
        private string _fk_device_id = "";

        /// <summary>
        /// 卡号
        /// </summary>
        public string fcode { get; set; }

        /// <summary>
        /// 卡状态 1:微信已经注册 2:挂失 3:注销 默认:0
        /// </summary>
        public int fstatus { get;set;}

        /// <summary>
        /// 学校名称
        /// </summary>
        public string school { get;set;}

        /// <summary>
        /// 卡片是否使用到期 0:不是 1:是
        /// </summary>
        public int fisovertime { get;set;}
    }
}
