using Senparc.NeuChar.Entities;
using Senparc.Weixin;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
// ReSharper disable NotAccessedField.Local


namespace SchoolBusWXWeb.CommServices.MessageHandlers.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// 把MessageHandler作为基类，重写对应请求的处理方法
    /// </summary>
    public class CustomMessageHandler : MessageHandler<CustomMessageContext>
    {
        private readonly string _appId = Config.SenparcWeixinSetting.WeixinAppId;
        public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0) : base(inputStream, postModel, maxRecordCount)
        {
            if (!string.IsNullOrEmpty(postModel.AppId))
            {
                _appId = postModel.AppId;//通过第三方开放平台发送过来的请求
            }
            //在指定条件下，不使用消息去重
            OmitRepeatedMessageFunc = requestMessage => !(requestMessage is RequestMessageText textRequestMessage) || textRequestMessage.Content != "容错";
        }


        public override async Task OnExecutingAsync(CancellationToken cancellationToken)
        {
            //测试MessageContext.StorageData
            if (CurrentMessageContext.StorageData == null || (CurrentMessageContext.StorageData is int))
            {
                CurrentMessageContext.StorageData = 0;
            }
            await base.OnExecutingAsync(cancellationToken);
        }

        public override async Task OnExecutedAsync(CancellationToken cancellationToken)
        {
            await base.OnExecutedAsync(cancellationToken);
            CurrentMessageContext.StorageData = ((int)CurrentMessageContext.StorageData) + 1;
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
                defaultResponseMessage.Content = $"{requestMessage.FromUserName},你刚才发送了消息Async。{requestMessage.Content}";
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
            if (requestMessage.EventKey == "key6")
            {
                return new ResponseMessageNoResponse();
            }
            return responseMessage;

        }

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
