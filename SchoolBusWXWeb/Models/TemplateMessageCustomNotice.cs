using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;

namespace SchoolBusWXWeb.Models
{
 
    public class TemplateMessageCustomNotice : TemplateMessageBase
    {
        public TemplateDataItem first { get; set; }
        public TemplateDataItem keyword1 { get; set; }
        public TemplateDataItem keyword2 { get; set; }
        public TemplateDataItem keyword3 { get; set; }
        public TemplateDataItem keyword4 { get; set; }
        public TemplateDataItem keyword5 { get; set; }
        public TemplateDataItem remark { get; set; }
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
            first = new TemplateDataItem(_first);
            keyword1 = new TemplateDataItem(openid);
            keyword2 = new TemplateDataItem(biaoti);
            keyword3 = new TemplateDataItem(neirong);
            keyword4 = new TemplateDataItem(neirong2);
            keyword5 = new TemplateDataItem(Messagetime);
            remark = new TemplateDataItem(_remark);
        }
    }
}
