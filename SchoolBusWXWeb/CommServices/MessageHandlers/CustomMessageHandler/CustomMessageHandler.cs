using Senparc.NeuChar.Entities;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageContexts;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SchoolBusWXWeb.CommServices.MessageHandlers.CustomMessageHandler
{

    /// <summary>
    /// 自定义MessageHandler
    /// 把MessageHandler作为基类，重写对应请求的处理方法
    /// </summary>
    public class CustomMessageHandler : MessageHandler<DefaultMpMessageContext>
    {
        private readonly string _appId = Config.SenparcWeixinSetting.WeixinAppId;

        /// <summary>
        /// 为中间件提供生成当前类的委托
        /// </summary>
        public static Func<Stream, PostModel, int, CustomMessageHandler> GenerateMessageHandler = (stream, postModel, maxRecordCount)
                        => new CustomMessageHandler(stream, postModel, maxRecordCount, false /* 是否只允许处理加密消息，以提高安全性 */);

        public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0, bool onlyAllowEcryptMessage = false)
            : base(inputStream, postModel, maxRecordCount, onlyAllowEcryptMessage)
        {
            //这里设置仅用于测试，实际开发可以在外部更全局的地方设置，
            //比如MessageHandler<MessageContext>.GlobalGlobalMessageContext.ExpireMinutes = 3。
            GlobalMessageContext.ExpireMinutes = 3;
            //OnlyAllowEcryptMessage = true;//是否只允许接收加密消息，默认为 false
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
            return await Task.Run<IResponseMessageBase>(() =>
            {
                if (requestMessage.Content.Contains("教程"))
                {
                    var responseMessage = CreateResponseMessage<ResponseMessageNews>();
                    responseMessage.Articles = new List<Article>
                    {
                        new Article
                        {
                            Title = "绑卡指南",
                            Description = "小鲸绑卡指南教程",
                            PicUrl = "http://wx.360wll.cn/img/pic1.jpg",
                            Url = "https://mp.weixin.qq.com/s/HRPdPiQqyGgoWKRud-N9-w"
                        }
                    };
                    return responseMessage;
                }
                else
                {
                    var defaultResponseMessage = CreateResponseMessage<ResponseMessageText>();
                    defaultResponseMessage.Content = $"你刚才发送了消息:{requestMessage.Content}";
                    return defaultResponseMessage;
                }
            });
        }

        /// <summary>
        /// 菜单按钮事件请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_ClickRequestAsync(RequestMessageEvent_Click requestMessage)
        {
            return await Task.Run<IResponseMessageBase>(() =>
            {
                var responseMessage = CreateResponseMessage<ResponseMessageNews>();
                if (requestMessage.EventKey != "key7") return new ResponseMessageNoResponse();
                responseMessage.Articles = new List<Article>
                {
                    new Article
                    {
                        Title = $"绑卡指南",
                        Description = "小鲸绑卡指南教程",
                        PicUrl = "http://bing.360wll.cn/20190622.JPG",
                        Url = "https://mp.weixin.qq.com/s/HRPdPiQqyGgoWKRud-N9-w"
                    },new Article
                    {
                        Title = $"开始注册绑卡吧",
                        Description = "注册用户",
                        PicUrl = "http://wx.360wll.cn/img/pic1.jpg",
                        Url = "http://wx.360wll.cn/SchoolBus/index?type=0"
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
                responseMessage.Content = $"小鲸欢迎您 {nickName} 的到来~\r\n点击下方菜单栏，了解“鲸卫士校车联盟”。绑定乘车卡请点击下方菜单栏中“我的校车”——“绑定新卡”按钮，完成相关信息的填写，或者直接点击下方蓝字 <a href=\"http://wx.360wll.cn/SchoolBus/index?type=0\">绑定新卡</a>如问问题和疑问，请随时与小鲸联系.\r\n客服电话 <a href=\"tel:024-62151515\">024-62151515</a>";
                return responseMessage;
            });
        }


        public override async Task<IResponseMessageBase> DefaultResponseMessageAsync(IRequestMessageBase requestMessage)
        {
            return await Task.Run(() =>
            {
                var responseMessage = CreateResponseMessage<ResponseMessageText>();
                responseMessage.Content = $"这条消息来自DefaultResponseMessageAsync.";
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
