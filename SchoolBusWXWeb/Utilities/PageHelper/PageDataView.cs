using System.Collections.Generic;

namespace SchoolBusWXWeb.Utilities.PageHelper
{
    public class PageDataView<T>
    {
        /// <summary>
        ///     总条数
        /// </summary>
        public int TotalNum { get; set; }

        /// <summary>
        ///     分页数据
        /// </summary>
        public IEnumerable<T> Items { get; set; } = new List<T>();

        /// <summary>
        ///     当前页
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        ///     总页数
        /// </summary>
        public int TotalPageCount { get; set; }
    }
}