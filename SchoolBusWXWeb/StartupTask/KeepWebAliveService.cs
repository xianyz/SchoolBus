using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolBusWXWeb.StartupTask
{
    public class KeepWebAliveService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
#if DEBUG
                const string url = "https://localhost:5001/home/index";
#else
                const string url="http://wx.360wll.cn/home/index";
#endif
                try
                {
                    Senparc.CO2NET.HttpUtility.RequestUtility.HttpGet(url, encoding: Encoding.UTF8);
                }
                catch (Exception)
                {
                    // ignored
                }
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
