using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolBusWXWeb.Models.ViewData;
using Senparc.CO2NET.Extensions;

namespace SchoolBusWXWeb.Filters
{
    //[CustomOAuth(null, "/OAuth2/UserInfoCallback", Senparc.Weixin.MP.OAuthScope.snsapi_base)]
    //public class OAuthBaseAttribute : Attribute, IActionFilter
    //{
    //    public string UserName { get; set; }
    //    public DateTime PageStartTime { get; set; }
    //    public void OnActionExecuting(ActionExecutingContext context)
    //    {
    //        UserName = context.HttpContext.Session.GetString("Userinfo");
    //    }

    //    public void OnActionExecuted(ActionExecutedContext context)
    //    {

           
    //    }
    //}
}
