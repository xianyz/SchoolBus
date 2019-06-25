using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Models.ViewData
{
    public class OAuthBaseVD
    {
        public string UserName { get; set; }
        public DateTime PageRenderTime { get; internal set; }
    }
}
