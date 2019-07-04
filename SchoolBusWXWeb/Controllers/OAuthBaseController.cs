using Microsoft.AspNetCore.Mvc;
using SchoolBusWXWeb.Filters;

namespace SchoolBusWXWeb.Controllers
{
    [CustomOAuth(null, "/OAuth2/UserInfoCallback", Senparc.Weixin.MP.OAuthScope.snsapi_base)]
    public class OAuthBaseController : Controller
    {
      
    }
}