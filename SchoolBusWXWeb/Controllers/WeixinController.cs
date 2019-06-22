using Microsoft.AspNetCore.Mvc;
using SchoolBusWXWeb.CommServices.MessageHandlers.CustomMessageHandler;
using Senparc.CO2NET.HttpUtility;
using Senparc.NeuChar.MessageHandlers;
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
        private static readonly string Token = Config.SenparcWeixinSetting.Token ?? CheckSignature.Token;//与微信公众账号后台的Token设置保持一致，区分大小写。
        private static readonly string EncodingAesKey = Config.SenparcWeixinSetting.EncodingAESKey;//与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。
        private static readonly string AppId = Config.SenparcWeixinSetting.WeixinAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。


        [HttpGet]
        [ActionName("Index")]
        public Task<ActionResult> Get(string signature, string timestamp, string nonce, string echostr)
        {
            return Task.Factory.StartNew(() =>
            {
                if (CheckSignature.Check(signature, timestamp, nonce, Token))
                {
                    return echostr; //返回随机字符串则表示验证通过
                }

                return "failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, Token) + "。" +
                       "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。";
            }).ContinueWith<ActionResult>(task => Content(task.Result));
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
                return new WeixinResult("参数错误！");
            }

            postModel.Token = Token;
            postModel.EncodingAESKey = EncodingAesKey; //根据自己后台的设置保持一致
            postModel.AppId = AppId; //根据自己后台的设置保持一致

            var cancellationToken = new CancellationToken();//给异步方法使用

            var messageHandler = new CustomMessageHandler(Request.GetRequestMemoryStream(), postModel, 10)
            {
                DefaultMessageHandlerAsyncEvent = DefaultMessageHandlerAsyncEvent.SelfSynicMethod//没有重写的异步方法将默认尝试调用同步方法中的代码（为了偷懒）
            };

            #region 设置消息去重

            /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
             * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
            messageHandler.OmitRepeatedMessage = true;//默认已经开启，此处仅作为演示，也可以设置为false在本次请求中停用此功能

            #endregion

            messageHandler.SaveRequestMessageLog();//记录 Request 日志（可选）

            await messageHandler.ExecuteAsync(cancellationToken); //执行微信处理过程（关键）

            messageHandler.SaveResponseMessageLog();//记录 Response 日志（可选）


            return new FixWeixinBugWeixinResult(messageHandler);
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
                int t1, t2, t3;
                System.Threading.ThreadPool.GetAvailableThreads(out t1, out t3);
                System.Threading.ThreadPool.GetMaxThreads(out t2, out t3);
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(0.1));
                var end = SystemTime.Now;
                var thread = System.Threading.Thread.CurrentThread;
                var result = string.Format("TId:{0}\tApp:{1}\tBegin:{2:mm:ss,ffff}\tEnd:{3:mm:ss,ffff}\tTPool：{4}",
                    thread.ManagedThreadId,
                    HttpContext.GetHashCode(),
                    begin,
                    end,
                    t2 - t1
                    );
                return Content(result);
            });
        }
    }
}