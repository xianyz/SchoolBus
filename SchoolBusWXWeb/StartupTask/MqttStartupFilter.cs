using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using SchoolBusWXWeb.Hubs;
using SchoolBusWXWeb.Models;
using SchoolBusWXWeb.Utilities;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable NotAccessedField.Local
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable ClassNeverInstantiated.Global

namespace SchoolBusWXWeb.StartupTask
{
    /// <summary>
    /// 暂时不符合mqtt业务逻辑用不到
    /// </summary>
    public class MqttStartupFilter : IStartupTask
    {
        private readonly MqttOption _option;
        private readonly IServiceProvider _serviceProvider;
        private readonly IApplicationLifetime _appLifetime;
        private readonly IHubContext<ChatHub> _chatHub;
        public MqttStartupFilter(IServiceProvider serviceProvider, IOptions<SiteConfig> option, IApplicationLifetime appLifetime, IHubContext<ChatHub> chatHub)
        {
            _chatHub = chatHub;
            _serviceProvider = serviceProvider;
            _appLifetime = appLifetime;
            _option = option.Value.MqttOption;
        }
        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
           await Task.CompletedTask;
        }
    }
}
