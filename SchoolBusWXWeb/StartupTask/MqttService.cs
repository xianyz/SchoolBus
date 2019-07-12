using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolBusWXWeb.StartupTask
{
    /// <summary>
    /// Mqtt启动服务(暂时不用 用appLifetime代替)
    /// 参考:https://thinkrethink.net/2018/07/12/injecting-a-scoped-service-into-ihostedservice/
    /// </summary>
    public class MqttService : BackgroundService
    {
        //private readonly IServiceScopeFactory _serviceScopeFactory;
        //public MqttService(IServiceScopeFactory serviceScopeFactory)
        //{
        //    _serviceScopeFactory = serviceScopeFactory;
        //}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
            //using (var scope = _serviceScopeFactory.CreateScope())
            //{
            //    //var scoped = scope.ServiceProvider.GetRequiredService<ISchoolBusBusines>();
            //    // await Tools.ConnectMqttServerAsync(scoped);
            //}
        }
    }
}
