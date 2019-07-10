using Microsoft.AspNetCore.Mvc;
using SchoolBusWXWeb.CommServices.MessageHandlers.CustomMessageHandler;
using Senparc.CO2NET.HttpUtility;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MvcExtension;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolBusWXWeb.Controllers
{
    public class WeixinController : Controller
    {
        private static readonly string Token = Config.SenparcWeixinSetting.Token; //与微信公众账号后台的Token设置保持一致，区分大小写。

        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://sdk.weixin.senparc.com/weixin
        /// </summary>
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(PostModel postModel, string echostr)
        {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }

            return Content("failed:" + postModel.Signature + "," + CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, Token) + "。" +
                           "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
        }


        /// <summary>
        /// 最简化的处理流程
        /// </summary>
        [HttpPost]
        [ActionName("Index")]
        public async Task<ActionResult> Post(PostModel postModel)
        {
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content("参数错误！");
            }

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            var messageHandler = new CustomMessageHandler(Request.GetRequestMemoryStream(), postModel, 10);

            messageHandler.SaveRequestMessageLog();//记录 Request 日志（可选）

            await messageHandler.ExecuteAsync(new CancellationToken());//执行微信处理过程（关键）这里用异步CustomMessageHandler里面也需要用异步

            messageHandler.SaveResponseMessageLog();//记录 Response 日志（可选）

            //return Content(messageHandler.ResponseDocument.ToString());//v0.7-
            //return new WeixinResult(messageHandler);//v0.8+
            return new FixWeixinBugWeixinResult(messageHandler);//为了解决官方微信5.0软件换行bug暂时添加的方法，平时用下面一个方法即可
        }

        /// <summary>
        /// 为测试并发性能而建
        /// </summary>
        /// <returns></returns>
        public Task<ActionResult> ForTest()
        {
            //异步并发测试（提供给单元测试使用）
            return Task.Factory.StartNew<ActionResult>(() =>
            {
                var begin = SystemTime.Now;
                ThreadPool.GetAvailableThreads(out int t1, out _);
                ThreadPool.GetMaxThreads(out var t2, out _);
                Thread.Sleep(TimeSpan.FromSeconds(0.1));
                var end = SystemTime.Now;
                var thread = Thread.CurrentThread;
                var result = $"TId:{thread.ManagedThreadId}\tApp:{HttpContext.GetHashCode()}\tBegin:{begin:mm:ss,ffff}\tEnd:{end:mm:ss,ffff}\tTPool：{t2 - t1}";
                return Content(result);
            });
        }
    }
}