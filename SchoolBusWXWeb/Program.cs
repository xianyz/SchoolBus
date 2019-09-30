using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SchoolBusWXWeb.StartupTask;
using System.Threading.Tasks;

namespace SchoolBusWXWeb
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunWithTasksAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
#if !DEBUG
                    webBuilder.UseUrls("http://0.0.0.0:5005/");
#endif
                    webBuilder.ConfigureLogging(builder => builder.AddFile());
                });
    }
}
