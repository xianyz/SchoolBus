using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SchoolBusWXWeb.Models
{

    public class TemplateMessageCustomNotice : TemplateMessageBase
    {
        public TemplateDataItem First { get; set; }
        public TemplateDataItem Keyword1 { get; set; }
        public TemplateDataItem Keyword2 { get; set; }
        public TemplateDataItem Keyword3 { get; set; }
        public TemplateDataItem Keyword4 { get; set; }
        public TemplateDataItem Keyword5 { get; set; }
        public TemplateDataItem Remark { get; set; }
        public TemplateMessageCustomNotice(
            string _first,
            string openid,
            string biaoti,
            string neirong,
            string neirong2,
            string Messagetime,
            string _remark,
            string url, string templateId = "6RzeJPMnzoPv5AyWTx2ezAhDEyKmbidf4JspdBNT4Io")
            : base(templateId, url, "测试格式")
        {
            First = new TemplateDataItem(_first);
            Keyword1 = new TemplateDataItem(openid);
            Keyword2 = new TemplateDataItem(biaoti);
            Keyword3 = new TemplateDataItem(neirong);
            Keyword4 = new TemplateDataItem(neirong2);
            Keyword5 = new TemplateDataItem(Messagetime);
            Remark = new TemplateDataItem(_remark);
        }
    }
}
