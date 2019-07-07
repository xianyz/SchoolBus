using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace SchoolBusWXWeb.Filters
{

    // OnActionExecuting->homeindex->OnActionExecuted->OnResultExecuting->return View()->OnResultExecuted
    public class PermissionRequiredAttribute : ActionFilterAttribute
    {
        //private readonly ISchoolBusBusines _schoolBusBusines;
        //public PermissionRequiredAttribute(ISchoolBusBusines schoolBusBusines)
        //{
        //    _schoolBusBusines = schoolBusBusines;
        //}
        public string Message { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            Console.WriteLine("执行OnActionExecuting");
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            Console.WriteLine("执行OnActionExecuted");
           
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            Console.WriteLine("执行OnResultExecuting");
           
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            Console.WriteLine("执行OnResultExecuted");
        }

    }

}
