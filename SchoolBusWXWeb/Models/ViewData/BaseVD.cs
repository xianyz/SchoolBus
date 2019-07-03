using System.Collections.Generic;
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
        public List<SchoolMode> data { get;set;}
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
}
