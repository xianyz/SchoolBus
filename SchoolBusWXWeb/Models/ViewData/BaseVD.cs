using System;
using System.Collections.Generic;

namespace SchoolBusWXWeb.Models.ViewData
{
    public class BaseVD
    {
        public int status { get; set; }
        public string msg { get; set; }
    }
    public class RegisVD : BaseVD { }

    public class SmsVD : BaseVD { }

    public class SchoolVD : BaseVD
    {
        public List<SchoolTypeList> data { get; set; }
    }

    public class SchoolBaseInfo
    {
        public int ftype { get; set; }
        public string text { get; set; }
        public string value { get; set; }
       
    }

    public class SchoolTypeList
    {
        public int ftype { get; set; }
        public List<SchoolValueText> schoolValues { get; set; }
    }
    public class SchoolValueText
    {
        public string text { get; set; }
        public string value { get; set; }
    }
}
