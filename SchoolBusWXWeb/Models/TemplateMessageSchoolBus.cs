using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace SchoolBusWXWeb.Models
{
    public class TemplateMessageSchoolBus : TemplateMessageBase
    {
        private TemplateDataItem first { get; set; }
        private TemplateDataItem keyword1 { get; set; }
        private TemplateDataItem keyword2 { get; set; }
        private TemplateDataItem keyword3 { get; set; }
        private TemplateDataItem remark { get; set; }
        public TemplateMessageSchoolBus(string title,
            string fname,
            string time,
            string fplatenumber,
            string remark,
            string templateId, string url, string templateName) : base(templateId, url, templateName)
        {
            first = new TemplateDataItem(title, "#000000");
            keyword1 = new TemplateDataItem(fname, "#000000");
            keyword2 = new TemplateDataItem(time, "#000000");
            keyword3 = new TemplateDataItem(fplatenumber, "#000000");
            this.remark = new TemplateDataItem(remark,"#ff0000");
        }
    }
}
