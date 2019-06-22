using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace SchoolBusWXWeb.StartupTask
{
    public interface IStartupTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStartupTask<T>(this IServiceCollection services)
            where T : class, IStartupTask
            => services.AddTransient<IStartupTask, T>();
    }
    public static class StartupTaskWebHostExtensions
    {
        public static async Task RunWithTasksAsync(this IWebHost webHost, CancellationToken cancellationToken = default)
        {
            var startupTasks = webHost.Services.GetServices<IStartupTask>();

            foreach (var startupTask in startupTasks)
            {
                await startupTask.ExecuteAsync(cancellationToken);
            }

            await webHost.RunAsync(cancellationToken);
        }
    }
}
