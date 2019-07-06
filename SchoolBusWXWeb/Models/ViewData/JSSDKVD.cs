using Senparc.Weixin.MP.Helpers;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global

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
