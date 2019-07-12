using Senparc.NeuChar.Entities;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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
        private readonly string _appId = Config.SenparcWeixinSetting.WeixinAppId;
        public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0) : base(inputStream, postModel, maxRecordCount)
        {
            base.CurrentMessageContext.ExpireMinutes = 10;
            if (!string.IsNullOrEmpty(postModel.AppId))
            {
                _appId = postModel.AppId;//通过第三方开放平台发送过来的请求
            }
            //在指定条件下，不使用消息去重
            OmitRepeatedMessageFunc = requestMessage => !(requestMessage is RequestMessageText textRequestMessage) || textRequestMessage.Content != "容错";
        }


        /// <summary>
        /// 文字
        /// </summary>
        /// <param name="requestMessage"></param>
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
        /// 菜单按钮事件请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_ClickRequestAsync(RequestMessageEvent_Click requestMessage)
        {
            return await Task.Run(() =>
            {
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
                return responseMessage;
            });
        }

        /// <summary>
        /// 用户第一次关注
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_SubscribeRequestAsync(RequestMessageEvent_Subscribe requestMessage)
        {
            return await Task.Run(() =>
            {
                var userInfo = UserApi.Info(_appId, OpenId);
                var nickName = userInfo?.nickname ?? "test";
                var responseMessage = CreateResponseMessage<ResponseMessageText>();
                responseMessage.Content = $"欢迎您 {nickName} 的到来";
                return responseMessage;
            });
        }


        public override async Task<IResponseMessageBase> DefaultResponseMessageAsync(IRequestMessageBase requestMessage)
        {
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
