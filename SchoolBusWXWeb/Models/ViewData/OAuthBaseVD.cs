using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;

// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Models.ViewData
{
    public class OAuthBaseVD
    {
        public OAuthUserInfo UserInfo { get; set; }
        public DateTime PageRenderTime { get;  set; }
    }
}
