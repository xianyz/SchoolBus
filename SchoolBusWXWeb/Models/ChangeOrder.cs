using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolBusWXWeb.Models
{
    public class ChangeOrder
    {
        /// <summary>
        /// 表名称
        /// </summary>
        public string Table { get; set; }
        /// <summary>
        /// 主键字段
        /// </summary>
        public string Keyfield { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderNumfield { get; set; }
        /// <summary>
        /// 交换数据的主键ID 跟Endid交换
        /// </summary>
        public int Startid { get; set; }
        /// <summary>
        /// 交换数据的主键ID 跟Startid交换
        /// </summary>
        public int Endid { get; set; }
    }
}
