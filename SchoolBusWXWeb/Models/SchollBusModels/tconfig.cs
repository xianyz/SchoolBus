using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    public class tconfig
    {
        [Key]
        public string pkid { get; set; } = Guid.NewGuid().ToString("N");
        /// <summary>
        /// 编码 
        /// </summary>
        public string fcode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string fname { get; set; }
        /// <summary>
        /// 值 如果 fcode=001 代表试用期是几天
        /// </summary>
        public string fvalue { get;set;}
        /// <summary>
        /// 备注
        /// </summary>
        public string fremark { get;set;}
    }
}
