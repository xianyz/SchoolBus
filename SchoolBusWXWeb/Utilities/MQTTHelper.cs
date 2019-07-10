using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using Newtonsoft.Json;
using SchoolBusWXWeb.Business;
using SchoolBusWXWeb.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBusWXWeb.Utilities
{
    public class MqttHelper
    {
        public static IMqttClient MqttClient;
        public static bool IsReconnect = true;
        private readonly MqttOption _option;
        private readonly ISchoolBusBusines _schoolBusBusines;
        public MqttHelper(ISchoolBusBusines schoolBusBusines, IOptions<SiteConfig> option)
        {
            _schoolBusBusines = schoolBusBusines;
            _option= option.Value.MqttOption;
        }

        public async Task ConnectMqttServerAsync()
        {
            IMqttClientOptions MqttOptions()
            {
                var options = new MqttClientOptionsBuilder()
                    .WithClientId(_option.ClientID)
                    .WithTcpServer(_option.HostIp, _option.HostPort)
                    .WithCredentials(_option.UserName, _option.Password)
                    //.WithTls()//服务器端没有启用加密协议，这里用tls的会提示协议异常
                    .WithCleanSession()
                    .Build();
                return options;
            }

            // Create a new Mqtt client.
            try
            {
                if (MqttClient == null)
                {
                    MqttClient = new MqttFactory().CreateMqttClient();

                    // 接收到消息回调
                    MqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(async e =>
                    {
                        var received = new MqttMessageReceived
                        {
                            Topic = e.ApplicationMessage.Topic,
                            Payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload),
                            QoS = e.ApplicationMessage.QualityOfServiceLevel,
                            Retain = e.ApplicationMessage.Retain
                        };
#if DEBUG
                        //var data = await _schoolBusBusines.GetTwxuserAsync("2c9ab45969dc19990169dd5bb9ea08b5");
                        //await Tools.WriteTxt("E:\\User.txt", JsonConvert.SerializeObject(data));
                        //const string path = "E:\\MQTTPayload.txt";
                        //await Tools.WriteTxt(path, received.Payload);
#endif
                        await _schoolBusBusines.MqttMessageReceivedAsync(received);
                        Console.WriteLine($">> ### 接受消息 ###{Environment.NewLine}");
                        Console.WriteLine($">> Topic = {received.Topic}{Environment.NewLine}");
                        Console.WriteLine($">> Payload = {received.Payload}{Environment.NewLine}");
                        Console.WriteLine($">> QoS = {received.QoS}{Environment.NewLine}");
                        Console.WriteLine($">> Retain = {received.Retain}{Environment.NewLine}");
                    });

                    // 连接成功回调
                    MqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(async e =>
                    {
                        Console.WriteLine("已连接到MQTT服务器！" + Environment.NewLine);
                        await Subscribe(MqttClient, _option.MqttTopic);
                    });
                    // 断开连接回调
                    MqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(async e =>
                    {
                        var curTime = DateTime.UtcNow;
                        Console.WriteLine($">> [{curTime.ToLongTimeString()}]");
                        Console.WriteLine("已断开MQTT连接！" + Environment.NewLine);
                        //Reconnecting 重连
                        if (IsReconnect && !e.ClientWasConnected)
                        {
                            Console.WriteLine("正在尝试重新连接" + Environment.NewLine);
                            await Task.Delay(TimeSpan.FromSeconds(5));
                            try
                            {
                                await MqttClient.ConnectAsync(MqttOptions());
                            }
                            catch
                            {
                                Console.WriteLine("### 重新连接 失败 ###" + Environment.NewLine);
                            }
                        }
                        else
                        {
                            Console.WriteLine("已下线！" + Environment.NewLine);
                        }
                    });

                    try
                    {
                        await MqttClient.ConnectAsync(MqttOptions());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("连接到MQTT服务器失败！" + Environment.NewLine + ex.Message + Environment.NewLine);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("连接到MQTT服务器未知异常！" + Environment.NewLine + e.Message + Environment.NewLine);
            }
        }
        private static async Task Subscribe(IMqttClient mqttClient, string topic)
        {
            if (mqttClient.IsConnected && !string.IsNullOrEmpty(topic))
            {
                // Subscribe to a topic
                await mqttClient.SubscribeAsync(new TopicFilterBuilder()
                    .WithTopic(topic)
                    .WithAtMostOnceQoS()
                    .Build()
                );
                Console.WriteLine($"已订阅[{topic}]主题{Environment.NewLine}");
            }
        }
    }
}
