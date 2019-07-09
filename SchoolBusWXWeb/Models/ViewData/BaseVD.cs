using System;
using System.Collections.Generic;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SchoolBusWXWeb.Models.ViewData
{
    public class BaseVD
    {
        public int status { get; set; }
        public string msg { get; set; }
    }

    public class RegisVD : BaseVD { }

    public class SaveCardInfoVD : BaseVD { }

    public class SmsVD : BaseVD { }

    public class SchoolVD : BaseVD
    {
        public List<SchoolMode> data { get; set; }
    }

    public class SchoolBaseInfo
    {
        public int ftype { get; set; }
        public string text { get; set; }
        public string value { get; set; }

    }

    public class SchoolMode
    {
        public string value { get; set; }
        public string text { get; set; }
        public List<SchoolValueText> children { get; set; }
    }

    public class SchoolValueText
    {
        private string _value;
        public string value
        {
            get => string.IsNullOrEmpty(_value) ? "" : _value.TrimEnd();
            set => _value = !string.IsNullOrEmpty(value) ? value : "";
        }
        public string text { get; set; }
    }

    public class AddressModel
    {
        /// <summary>
        /// 用户状态标识
        /// </summary>
        public int status { get; set; } = 99;

        /// <summary>
        /// 学生姓名
        /// </summary>
        public string student { get; set; }

        /// <summary>
        /// 0:刷卡位置 1:实时位置
        /// </summary>
        public int showType { get; set; }

        /// <summary>
        /// 刷卡位置:public.tcardlog 表 主键
        /// </summary>
        public string cardLogId { get; set; }

        /// <summary>
        /// public.tdevice 表的 设备编码
        /// </summary>
        public string fcode { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public decimal? flat { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public decimal? flng { get; set; }

        /// <summary>
        /// 微信分享链接
        /// </summary>
        public string wxLink { get; set; }

        /// <summary>
        /// 微信分享图标
        /// </summary>
        public string wximgUrl { get; set; }

        /// <summary>
        /// 微信分享标题
        /// </summary>
        public string wxshareTitle { get; set; }

        /// <summary>
        /// 微信分享描述
        /// </summary>
        public string wxshareDescription { get; set; }
    }

    public class IndexModel
    {
        /// <summary>
        /// 页面参数
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 用户状态码
        /// </summary>
        public int code { get; set; }
    }

    public class MonthCardLogModel
    {
        /// <summary>
        /// 几号打卡
        /// </summary>
        public int creatday { get; set; }

        /// <summary>
        /// 当天打卡次数
        /// </summary>
        public int count { get; set; }
    }

    public class CalendarVD : BaseVD
    {
        public Dictionary<string, string> monthMark { get; } = new Dictionary<string, string>();
        public List<DayCardLogModel> dayList { get; set; } = new List<DayCardLogModel>();
    }

    public class DayCardLogModel
    {
        /// <summary>
        /// cardLogId
        /// </summary>
        public string pkid
        {
            get => string.IsNullOrEmpty(_pkid) ? Guid.NewGuid().ToString("N") : _pkid.TrimEnd();
            set => _pkid = !string.IsNullOrEmpty(value) ? value : Guid.NewGuid().ToString("N");
        }
        private string _pkid;

        private DateTime fcreatetime { get; set; }

        public string formatcreatetime => fcreatetime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public class MqttMessage
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string pkid { get; set; }
        /// <summary>
        /// 设备号 tdevice表fcode
        /// </summary>
        public string dev_id { get; set; }
        /// <summary>
        /// 上下车状态 1:上车 2:下车
        /// </summary>
        public string sxc_zt { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string jd { get; set; }
        /// <summary>
        /// 维度
        /// </summary>
        public string wd { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public string gd { get; set; }
        /// <summary>
        /// 卡号数量
        /// </summary>
        public string card_num { get; set; }
        /// <summary>
        /// 卡号集合
        /// </summary>
        public List<string> card_list { get; set; }
    }

    /// <summary>
    /// 微信绑定卡记录
    /// </summary>
    public class UserBindCard
    {
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string fname { get; set; }
        /// <summary>
        /// 微信openid
        /// </summary>
        public string fwxid { get; set; }
        /// <summary>
        /// 校车牌号
        /// </summary>
        public string fplatenumber { get; set; }
        /// <summary>
        /// 司机姓名
        /// </summary>
        public string fdriver { get; set; }
        /// <summary>
        /// 司机手机号
        /// </summary>
        public string fdriverphone { get; set; }
        /// <summary>
        /// 卡号 卡编码
        /// </summary>
        public string fid { get; set; }

        /// <summary>
        /// 付款状态
        /// </summary>
        public int paystate { get; set; }

        /// <summary>
        /// 当天收到模板消息数量
        /// </summary>
        public int wxmsgcount { get; set; }
    }
}
