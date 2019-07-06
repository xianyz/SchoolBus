using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global

// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Models.ViewData
{
    public class OAuthBaseVD
    {
        public OAuthAccessTokenResult TokenResult { get;set;}
        public DateTime PageRenderTime { get;  set; }
    }
}
