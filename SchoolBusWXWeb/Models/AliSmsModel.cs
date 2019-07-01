using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolBusWXWeb.Models
{
    public class AliSmsModel
    {
        public string Message { get; set; }
        public string RequestId { get; set; }
        public string BizId { get; set; }
        public string Code { get; set; }
    }
}
