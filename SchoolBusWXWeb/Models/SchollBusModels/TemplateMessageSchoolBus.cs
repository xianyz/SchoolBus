using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace SchoolBusWXWeb.Models.SchollBusModels
{
    public class TemplateMessageSchoolBus : TemplateMessageBase
    {
        private TemplateDataItem First { get; set; }
        private TemplateDataItem Keyword1 { get; set; }
        private TemplateDataItem Keyword2 { get; set; }
        private TemplateDataItem Keyword3 { get; set; }
        private TemplateDataItem Keyword4 { get; set; }
        private TemplateDataItem Keyword5 { get; set; }
        private TemplateDataItem Remark { get; set; }
        public TemplateMessageSchoolBus(string first,
            string fname,
            string time,
            string fdriver,
            string fdriverphone,
            string fplatenumber,
            string remark,
            string templateId, string url, string templateName) : base(templateId, url, templateName)
        {
            First = new TemplateDataItem(first, "#000000");
            Keyword1 = new TemplateDataItem(fname, "#000000");
            Keyword2 = new TemplateDataItem(time, "#000000");
            Keyword3 = new TemplateDataItem(fdriver, "#000000");
            Keyword4 = new TemplateDataItem(fdriverphone, "#000000");
            Keyword5 = new TemplateDataItem(fplatenumber, "#000000");
            Remark = new TemplateDataItem(remark,"#ff0000");
        }
    }
}
