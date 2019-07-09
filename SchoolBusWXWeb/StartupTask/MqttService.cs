using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SchoolBusWXWeb.Business;
using SchoolBusWXWeb.Utilities;

namespace SchoolBusWXWeb.StartupTask
{
    /// <summary>
    /// 参考:https://thinkrethink.net/2018/07/12/injecting-a-scoped-service-into-ihostedservice/
    /// </summary>
    public class MqttService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public MqttService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var scoped = scope.ServiceProvider.GetRequiredService<ISchoolBusBusines>();
                await Tools.ConnectMqttServerAsync(scoped);
            }
        }
    }
}
