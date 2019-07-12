using SchoolBusWXWeb.Models;
using Senparc.NeuChar.Entities;
using Senparc.NeuChar.Entities.Request;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SchoolBusWXWeb.CommServices.MessageHandlers.MessageQueue;
// ReSharper disable RedundantToStringCallForValueType
// ReSharper disable RedundantToStringCall
// ReSharper disable NotAccessedField.Local


namespace SchoolBusWXWeb.CommServices.MessageHandlers.CustomMessageHandler
{

    /// <summary>
    /// 自定义MessageHandler
    /// 把MessageHandler作为基类，重写对应请求的处理方法
    /// </summary>
    public class CustomMessageHandler : MessageHandler<CustomMessageContext>
    {
        private DateTime StartTime { get; set; }
        private DateTime EndTime { get; set; }

        private readonly string _appId = Config.SenparcWeixinSetting.WeixinAppId;

        public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0) : base(inputStream, postModel, maxRecordCount)
        {
            StartTime = DateTime.Now;
            base.CurrentMessageContext.ExpireMinutes = 10; // 设置CurrentMessageContext.StorageData 过期时间
            if (!string.IsNullOrEmpty(postModel.AppId))
            {
                _appId = postModel.AppId;//通过第三方开放平台发送过来的请求
            }
            //在指定条件下，不使用消息去重
            OmitRepeatedMessageFunc = requestMessage => !(requestMessage is RequestMessageText textRequestMessage) || textRequestMessage.Content != "容错";
        }

        // 前置消息过滤
        //public override async Task OnExecutingAsync(CancellationToken cancellationToken)
        //{
        //    //测试MessageContext.StorageData
        //    if (CurrentMessageContext.StorageData is StorageModel storagemodel)
        //    {
        //        storagemodel.CMDCount++;
        //    }
        //    // CancelExcute=true; // 消息就不会向下执行,用于某些情况过滤
        //    await base.OnExecutingAsync(cancellationToken);
        //}

        /// <summary>
        /// 后置消息处理 (比如给消息加上签名) 已经对数据操作这里就不能处理了
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task OnExecutedAsync(CancellationToken cancellationToken)
        {
            if (ResponseMessage is ResponseMessageText responseMessage)
            {
                responseMessage.Content += "\r\n[刘哲测试公众号]";
            }
            // 超过5s中的处理 这里可以用队列
            await base.OnExecutedAsync(cancellationToken);

            #region 如果处理超时容灾方法

            //await Task.Delay(5000, cancellationToken);
            //EndTime = DateTime.Now;
            //var runTime = (EndTime - StartTime).TotalSeconds;
            //if (runTime > 4)
            //{
            //    var queueHandler = new MessageQueueHandler();

            //    ResponseMessage = await queueHandler.SendMessage(_appId, OpenId, ResponseMessage);
            //}

            #endregion

        }

        /// <summary>
        /// 处理文字请求
        /// </summary>
        /// <param name="requestMessage">请求消息</param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnTextRequestAsync(RequestMessageText requestMessage)
        {
            return await Task.Run(() =>
            {

                var defaultResponseMessage = CreateResponseMessage<ResponseMessageText>();
                var count = 0;
                if (CurrentMessageContext.StorageData is StorageModel storagemodeltemp)
                {
                    count = storagemodeltemp.CMDCount;
                }
                defaultResponseMessage.Content = $"{requestMessage.FromUserName},你刚才发送了消息Async。{requestMessage.Content},cmdcount:{count}";
                try
                {
                    var requestHandler = requestMessage.StartHandler()
                        .Keyword("cmd", () =>
                         {
                             CurrentMessageContext.StorageData = new StorageModel
                             {
                                 IsCMD = true
                             };
                             return defaultResponseMessage;
                         }).Keyword("exit", () =>
                         {
                             if (CurrentMessageContext.StorageData is StorageModel storagemodel)
                             {
                                 storagemodel.IsCMD = false;
                             }
                             return defaultResponseMessage;
                         }).Default(() => defaultResponseMessage);
                    return requestHandler.GetResponseMessage();
                }
                catch (Exception e)
                {
                    defaultResponseMessage.Content = "异常:" + e.ToString();
                }
                return defaultResponseMessage;
            });
        }

        /// <summary>
        /// 处理视频请求
        /// </summary>
        /// <param name="requestMessageVideo"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnVideoRequestAsync(RequestMessageVideo requestMessageVideo)
        {
            return await Task.Run(() =>
            {
                var responseMessage = CreateResponseMessage<ResponseMessageText>();
                responseMessage.Content = "您刚才发送的是小视频";
                return responseMessage;
            });
        }

        /// <summary>
        /// 菜单按钮事件请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_ClickRequestAsync(RequestMessageEvent_Click requestMessage)
        {
            await Task.CompletedTask;
            var responseMessage = CreateResponseMessage<ResponseMessageNews>();
            responseMessage.Articles = new List<Article>
            {
                new Article
                {
                    Title = $"第一条你刚才点击了按钮{requestMessage.EventKey}",
                    Description = "第一条带连接的内容",
                    PicUrl = "http://bing.360wll.cn/20190622.JPG",
                    Url = "http://metro.360wll.cn/Bing/Index"
                },new Article
                {
                    Title = $"第二条你刚才点击了按钮{requestMessage.EventKey}",
                    Description = "第二条带连接的内容",
                    PicUrl = "http://bing.360wll.cn/20190621.JPG",
                    Url = "http://metro.360wll.cn/Bing/Index"
                }
            };
            if (requestMessage.EventKey == "key2") // 刷卡记录
            {
                var defaultResponseMessage = CreateResponseMessage<ResponseMessageText>();
                if (CurrentMessageContext.StorageData is StorageModel storagemodel)
                {
                    defaultResponseMessage.Content = storagemodel.IsCMD ? "当前已经进入CMD状态" : "当前已经退出CMD状态";
                }
                else
                {
                    defaultResponseMessage.Content = "找不到StorageData";
                }
                return defaultResponseMessage;
            }
            if (requestMessage.EventKey == "key6") // 实时位置 不返回消息
            {
                return new ResponseMessageNoResponse();
            }
            // 可以用客服消息冒充
            await CustomApi.SendTextAsync(_appId, OpenId, "服务器发来的客服消息");
            return responseMessage;

        }

        /// <summary>
        /// 用户第一次关注
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_SubscribeRequestAsync(RequestMessageEvent_Subscribe requestMessage)
        {
            var userInfo = await UserApi.InfoAsync(_appId, OpenId);
            var nickName = userInfo?.nickname ?? "test";
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = $"欢迎您 {nickName} 的到来";
            return responseMessage;
        }

        /// <summary>
        /// 发送模板消息后的微信回调请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_TemplateSendJobFinishRequestAsync(RequestMessageEvent_TemplateSendJobFinish requestMessage)
        {
            if (requestMessage.Status == "success")
            {
                // 进行逻辑处理 看是否成功处理等
                await CustomApi.SendTextAsync(_appId, OpenId, "模板消息发送成功" + requestMessage.MsgID.ToString());
            }
            return new SuccessResponseMessage();
        }

        /// <summary>
        /// 默认消息对没有对应类型消息的默认处理
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> DefaultResponseMessageAsync(IRequestMessageBase requestMessage)
        {
            /* 所有没有被处理的消息会默认返回这里的结果，
            * 因此，如果想把整个微信请求委托出去（例如需要使用分布式或从其他服务器获取请求），
            * 只需要在这里统一发出委托请求，如：
            * var responseMessage = MessageAgent.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
            * return responseMessage;
            */
            return await Task.Run(() =>
            {
                 var responseMessage = CreateResponseMessage<ResponseMessageText>();
                 responseMessage.Content = $"这条消息来自DefaultResponseMessageAsync。{requestMessage.MsgType}";
                 return responseMessage;
            });
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = $"这条消息来自DefaultResponseMessage。{requestMessage.MsgType}";
            return responseMessage;
        }
    }
}
