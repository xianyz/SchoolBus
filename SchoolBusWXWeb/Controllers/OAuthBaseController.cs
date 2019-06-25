using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolBusWXWeb.Filters;
using SchoolBusWXWeb.Models.ViewData;

namespace SchoolBusWXWeb.Controllers
{
    [CustomOAuth(null, "/OAuth2/UserInfoCallback", Senparc.Weixin.MP.OAuthScope.snsapi_base)]
    public class OAuthBaseController : Controller
    {
      
    }
}