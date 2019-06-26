using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SchoolBusWXWeb.Utilities.PageHelper
{
    /// <summary>
    /// 分页标签
    /// </summary>
    [HtmlTargetElement("pager")]
    public class PagerTagHelper : TagHelper
    {
        // 属性名称是PagerOption对应pager-option这个细节不容忽视
        public MoPagerOption PagerOption { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (!(PagerOption?.TotalPageCount > 1)) return base.ProcessAsync(context, output);
            var divstyle = PagerOption.StyleNum == 2 ? "layui-box layui-laypage layui-laypage-default" : "page";
            output.TagName = "div";
            output.Attributes.SetAttribute("class", $"{divstyle}");
            PagerOption.CurrentPage = PagerOption.CurrentPage < 1 ? 1 : PagerOption.CurrentPage;
            var url = PagerOption.RouteUrl;
            if (url.IndexOf('#') > -1) url = url.Substring(0, url.IndexOf('#'));
            var sbPage = new StringBuilder("\r\n<div>");

            #region 首页

            if (PagerOption.TotalPageCount > 1)
                switch (PagerOption.StyleNum)
                {
                    case 1:
                        sbPage.Append("<a href=\"" + GetUrl(url, 1) + "\" class=\"num\">Top</a>\r\n");
                        break;
                    case 2:
                        sbPage.Append("<a href=\"" + GetUrl(url, 1) + "\" class=\"laypage_first\">首页</a>");
                        break;
                    default:
                        sbPage.Append("<a href=\"" + GetUrl(url, 1) + "\" class=\"num\">Top</a>\r\n");
                        break;
                }

            #endregion 首页

            #region 上一页

            if (PagerOption.CurrentPage >= 2)
                switch (PagerOption.StyleNum)
                {
                    case 1:
                        sbPage.Append("<a href='" + GetUrl(url, PagerOption.CurrentPage - 1) +
                                      "' class=\"prev\" > << </a>\r\n");
                        break;
                    case 2:
                        sbPage.Append("<a href='" + GetUrl(url, PagerOption.CurrentPage - 1) +
                                      "' class=\"layui-laypage-prev\" >上一页</a>");
                        break;
                    default:
                        sbPage.Append("<a href='" + GetUrl(url, PagerOption.CurrentPage - 1) +
                                      "' class=\"prev\" > << </a>\r\n");
                        break;
                }

            #endregion 上一页

            #region 中间和当前页

            var half = PagerOption.ViewCount % 2 == 0
                ? PagerOption.ViewCount / 2
                : PagerOption.ViewCount / 2 + 1; // 展示页一半
            var leng = PagerOption.ViewCount;
            if (PagerOption.ViewCount > PagerOption.TotalPageCount) leng = PagerOption.TotalPageCount;

            if (PagerOption.CurrentPage > PagerOption.TotalPageCount - leng + half)
                for (var i = PagerOption.TotalPageCount - leng + 1; i <= PagerOption.TotalPageCount; i++)
                    if (i == PagerOption.CurrentPage)
                        switch (PagerOption.StyleNum)
                        {
                            case 1:
                                sbPage.Append("<span class=\"current\">" + PagerOption.CurrentPage + "</span>\r\n");
                                break;
                            case 2:
                                sbPage.Append(
                                    "<span class=\"layui-laypage-curr\"><em class=\"layui-laypage-em\"></em><em>" +
                                    PagerOption.CurrentPage + "</em></span>");
                                break;
                            default:
                                sbPage.Append("<span class=\"current\">" + PagerOption.CurrentPage + "</span>\r\n");
                                break;
                        }
                    else
                        switch (PagerOption.StyleNum)
                        {
                            case 1:
                                sbPage.Append("<a href='" + GetUrl(url, i) + "' class=\"num\">" + i + "</a>\r\n");
                                break;
                            case 2:
                                sbPage.Append("<a href='" + GetUrl(url, i) + "'>" + i + "</a>");
                                break;
                            default:
                                sbPage.Append("<a href='" + GetUrl(url, i) + "' class=\"num\">" + i + "</a>\r\n");
                                break;
                        }
            else
                for (var i = 1; i <= leng; i++)
                    if (i == half)
                    {
                        switch (PagerOption.StyleNum)
                        {
                            case 1:
                                sbPage.Append("<span class=\"current\">" + PagerOption.CurrentPage + "</span>\r\n");
                                break;
                            case 2:
                                sbPage.Append(
                                    "<span class=\"layui-laypage-curr\"><em class=\"layui-laypage-em\"></em><em>" +
                                    PagerOption.CurrentPage + "</em></span>");
                                break;
                            default:
                                sbPage.Append("<span class=\"current\">" + PagerOption.CurrentPage + "</span>\r\n");
                                break;
                        }
                    }
                    else
                    {
                        var p = PagerOption.CurrentPage - half + i;
                        if (p > 0)
                            switch (PagerOption.StyleNum)
                            {
                                case 1:
                                    sbPage.Append("<a href='" + GetUrl(url, p) + "' class=\"num\" >" + p +
                                                  "</a>\r\n");
                                    break;
                                case 2:
                                    sbPage.Append("<a href='" + GetUrl(url, p) + "'>" + p + "</a>");
                                    break;
                                default:
                                    sbPage.Append("<a href='" + GetUrl(url, p) + "' class=\"num\" >" + p +
                                                  "</a>\r\n");
                                    break;
                            }
                        else
                            leng += 1;
                    }

            #endregion 中间和当前页

            #region 下一页

            if (PagerOption.CurrentPage < PagerOption.TotalPageCount)
                switch (PagerOption.StyleNum)
                {
                    case 1:
                        sbPage.Append("<a href='" +
                                      GetUrl(url, PagerOption.CurrentPage < 2 ? 2 : PagerOption.CurrentPage + 1) +
                                      "' class=\"next\" > >> </a> \r\n");
                        break;
                    case 2:
                        sbPage.Append("<a href='" +
                                      GetUrl(url, PagerOption.CurrentPage < 2 ? 2 : PagerOption.CurrentPage + 1) +
                                      "' class=\"layui-laypage-next\" >下一页</a>");
                        break;
                    default:
                        sbPage.Append("<a href='" +
                                      GetUrl(url, PagerOption.CurrentPage < 2 ? 2 : PagerOption.CurrentPage + 1) +
                                      "' class=\"next\" > >> </a> \r\n");
                        break;
                }

            #endregion 下一页

            #region 末页

            if (PagerOption.TotalPageCount > 1)
                switch (PagerOption.StyleNum)
                {
                    case 1:
                        sbPage.Append("<a href=\"" + GetUrl(url, PagerOption.TotalPageCount) +
                                      "\" class=\"num\">Last</a>\r\n");
                        break;
                    case 2:
                        sbPage.Append("<a href=\"" + GetUrl(url, PagerOption.TotalPageCount) +
                                      "\" class=\"layui-laypage-last\">末页</a>");
                        break;
                    default:
                        sbPage.Append("<a href=\"" + GetUrl(url, PagerOption.TotalPageCount) +
                                      "\" class=\"num\">Last</a>\r\n");
                        break;
                }

            #endregion 末页

            sbPage.Append("</div>");
            output.Content.SetHtmlContent(sbPage.ToString());

            return base.ProcessAsync(context, output);
        }

        private static string GetUrl(string url, int pageIndex)
        {
            string newUrl;
            const string pattern = "page=([+-]?\\d+)";
            if (Regex.Match(url, pattern).Success)
            {
                newUrl = Regex.Replace(url, pattern, "page=" + pageIndex);
            }
            else
            {
                var fag = url.IndexOf('?') > -1 ? "&" : "?";
                newUrl = string.Format(url + "{0}", fag + "page=" + pageIndex);
            }

            return newUrl;
        }
    }

    /// <summary>
    ///     分页option属性
    /// </summary>
    public class MoPagerOption
    {
        /// <summary>
        ///     当前页  必传
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        ///     总条数  必传
        /// </summary>
        public int TotalNum { get; set; }

        /// <summary>
        ///     总页数
        /// </summary>
        public int TotalPageCount { get; set; }

        /// <summary>
        ///     分页记录数（每页条数 默认每页10条）
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///     路由地址(格式如：/Controller/Action) 默认自动获取
        /// </summary>
        public string RouteUrl { get; set; }

        /// <summary>
        ///     展示的页数
        /// </summary>
        public int ViewCount { get; set; } = 5;

        /// <summary>
        ///     样式 默认 bootstrap样式 1
        /// </summary>
        public int StyleNum { get; set; }
    }
}