using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using SchoolBusWXWeb.Models;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable ClassNeverInstantiated.Global

namespace SchoolBusWXWeb.StartupTask
{
    public class MqttStartupFilter : IStartupTask
    {
        private readonly MqttOption _option;

        private readonly IServiceProvider _serviceProvider;
        public MqttStartupFilter(IServiceProvider serviceProvider, IOptions<SiteConfig> option)
        {
            _serviceProvider = serviceProvider;
            _option = option.Value.mqttOption;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            using (_serviceProvider.CreateScope())
            {
                await ConnectMqttServerAsync();
            }
        }

        private async Task ConnectMqttServerAsync()
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
                var mqttClient = new MqttFactory().CreateMqttClient();
                mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e =>
                {
                    Console.WriteLine($">> ### 接受消息 ###{Environment.NewLine}");
                    Console.WriteLine($">> Topic = {e.ApplicationMessage.Topic}{Environment.NewLine}");
                    Console.WriteLine($">> Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}{Environment.NewLine}");
                    Console.WriteLine($">> QoS = {e.ApplicationMessage.QualityOfServiceLevel}{Environment.NewLine}");
                    Console.WriteLine($">> Retain = {e.ApplicationMessage.Retain}{Environment.NewLine}");

                });
                mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(async e =>
                {
                    Console.WriteLine("已连接到MQTT服务器！" + Environment.NewLine);
                    await Subscribe(mqttClient, _option.MqttTopic);
                });

                mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(e =>
                {
                    var curTime = DateTime.UtcNow;
                    Console.WriteLine($">> [{curTime.ToLongTimeString()}]");
                    Console.WriteLine("已断开MQTT连接！" + Environment.NewLine);
                    // 应该做重试
                });

                try
                {
                    await mqttClient.ConnectAsync(MqttOptions());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("连接到MQTT服务器失败！" + Environment.NewLine + ex.Message + Environment.NewLine);
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
