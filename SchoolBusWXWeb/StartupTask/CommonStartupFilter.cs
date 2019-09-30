using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SchoolBusWXWeb.Models;
using SchoolBusWXWeb.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolBusWXWeb.StartupTask
{
    public class CommonStartupFilter : IStartupTask
    {
        private readonly IServiceProvider _serviceProvider;

        public CommonStartupFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                using IServiceScope scope = _serviceProvider.CreateScope();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                var settings = configuration.GetSection("SiteConfig").Get<SiteConfig>();
                Tools.SetUtilsProviderConfiguration(settings, logger);
            });
        }

    }
}
