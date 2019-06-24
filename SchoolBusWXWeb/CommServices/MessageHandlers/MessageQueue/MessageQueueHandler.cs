using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Senparc.CO2NET.MessageQueue;
using Senparc.NeuChar.Entities;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Entities;

namespace SchoolBusWXWeb.CommServices.MessageHandlers.MessageQueue
{
    public class MessageQueueHandler
    {
        public async Task<IResponseMessageBase> SendMessage(string appid, string openid, IResponseMessageBase responseMessage)
        {
            var messageQueue = new SenparcMessageQueue();
            if (!(responseMessage is ResponseMessageText responseMessagea)) return responseMessage;
            return await Task.Run(() =>
            {
                {
                    var key = SenparcMessageQueue.GenerateKey("MessageHandlerSendMessageAsync", responseMessage.GetType(), Guid.NewGuid().ToString(), "SendMessage");
                    messageQueue.Add(key, () =>
                    {
                        responseMessagea.Content += "\r\n[消息超时后客服发送的消息1]";
                        // 发送客服消息 在队列里面就可以不用 async await 了
                        CustomApi.SendText(appid, openid, responseMessagea.Content);
                    });
                }

                {
                    var key = SenparcMessageQueue.GenerateKey("MessageHandlerSendMessageAsync", responseMessage.GetType(), Guid.NewGuid().ToString(), "SendMessage");
                    messageQueue.Add(key, () =>
                    {
                        responseMessagea.Content += "\r\n[消息超时后客服发送的消息2]";
                        // 发送客服消息 在队列里面就可以不用 async await 了
                        CustomApi.SendText(appid, openid, responseMessagea.Content);
                    });
                }
                return new ResponseMessageNoResponse();
            });
        }
    }
}
