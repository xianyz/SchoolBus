using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Senparc.Weixin.MP.Helpers;
// ReSharper disable InconsistentNaming

namespace SchoolBusWXWeb.Models.ViewData
{
    public class Base_JSSDKVD : OAuthBaseVD
    {
        public JsSdkUiPackage JsSdkUiPackage { get; set; }
    }

    public class JSSDK_Index : Base_JSSDKVD
    {
        public string Msg { get; set; }
    }
}
