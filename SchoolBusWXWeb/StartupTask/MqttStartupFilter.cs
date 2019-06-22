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
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable ClassNeverInstantiated.Global

namespace SchoolBusWXWeb.StartupTask
{
    public class MqttStartupFilter : IStartupTask
    {
        private IMqttClient _mqttClient;
        private bool _isReconnect = true;
        private readonly MqttOption _option;
        private readonly IServiceProvider _serviceProvider;
        private readonly IApplicationLifetime _appLifetime;
        public MqttStartupFilter(IServiceProvider serviceProvider, IOptions<SiteConfig> option, IApplicationLifetime appLifetime)
        {
            _serviceProvider = serviceProvider;
            _appLifetime = appLifetime;
            _option = option.Value.MqttOption;
        }
        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            await ConnectMqttServerAsync();

            #region 控制程序生命周期
            // 发生在应用启动成功以后，也就是Startup.Configure()方法结束后。
            _appLifetime.ApplicationStarted.Register(() =>
            {

            });
            // 发生在程序正在执行退出的过程中，此时还有请求正在被处理。应用程序也会等到这个事件完成后，再退出。
            _appLifetime.ApplicationStopping.Register(() =>
            {

            });
            // 发生在程序正在完成正常退出的时候，所有请求都被处理完成。程序会在处理完这货的Action委托代码以后退出
            _appLifetime.ApplicationStopped.Register(async () =>
            {
                _isReconnect = false;
                await _mqttClient.DisconnectAsync();
            });
            #endregion
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
                if (_mqttClient == null)
                {
                    _mqttClient = new MqttFactory().CreateMqttClient();
                    
                    _mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e =>
                    {
                        Console.WriteLine($">> ### 接受消息 ###{Environment.NewLine}");
                        Console.WriteLine($">> Topic = {e.ApplicationMessage.Topic}{Environment.NewLine}");
                        Console.WriteLine($">> Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}{Environment.NewLine}");
                        Console.WriteLine($">> QoS = {e.ApplicationMessage.QualityOfServiceLevel}{Environment.NewLine}");
                        Console.WriteLine($">> Retain = {e.ApplicationMessage.Retain}{Environment.NewLine}");

                    });
                   
                    _mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(async e =>
                    {
                        Console.WriteLine("已连接到MQTT服务器！" + Environment.NewLine);
                        await Subscribe(_mqttClient, _option.MqttTopic);
                    });

                    _mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(async e =>
                    {
                        var curTime = DateTime.UtcNow;
                        Console.WriteLine($">> [{curTime.ToLongTimeString()}]");
                        Console.WriteLine("已断开MQTT连接！" + Environment.NewLine);
                        //Reconnecting 重连
                        if (_isReconnect)
                        {
                            Console.WriteLine("正在尝试重新连接" + Environment.NewLine);
                            await Task.Delay(TimeSpan.FromSeconds(5));
                            try
                            {
                                await _mqttClient.ConnectAsync(MqttOptions());
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
                        await _mqttClient.ConnectAsync(MqttOptions());
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
