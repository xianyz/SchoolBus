using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SchoolBusWXWeb.Filters
{
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        private readonly IHostingEnvironment _env;
        private readonly ILoggerFactory _loggerFactory; // 自带日志

        //构造函数注入ILoggerHelper
        public GlobalExceptionFilter(ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            _env = env;
            _loggerFactory = loggerFactory;
        }
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            await Task.Run(() =>
             {
                 var logger = _loggerFactory.CreateLogger(context.Exception.TargetSite.ReflectedType);
                 var json = new ErrorResponse("未知错误,请重试")
                 {
                     Name = context.ActionDescriptor.GetType().GetProperty("ActionName")?.GetValue(context.ActionDescriptor).ToString(),
                     Path = $"链接访问出错：{context.HttpContext.Request.Path}",
                     Msg = context.Exception.Message,
                     Data = context.Exception
                 };
                 if (context.Exception is OperationCanceledException)
                 {
                     json.Msg = "一个请求在浏览器被取消";
                     if (_env.IsDevelopment()) logger.LogInformation("请求被取消了");
                 }
                 else
                 {
                     logger.LogError(new EventId(context.Exception.HResult), context.Exception, JsonConvert.SerializeObject(json));
                 }
                 context.Result = new RedirectResult("/Home/Error");
                 context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                 context.ExceptionHandled = true; //设置异常已经处理,否则会被其他异常过滤器覆盖
             });
        }
    }
    public class ApplicationErrorResult : ObjectResult
    {
        public ApplicationErrorResult(object value) : base(value)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
    public class ErrorResponse
    {
        public ErrorResponse(string msg)
        {
            Msg = msg;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; } = 0;

        public string Name { get; set; }
        public string Path { get; set; }
        public string Msg { get; set; }
        public object Data { get; set; }
    }
}
