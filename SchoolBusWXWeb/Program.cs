using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SchoolBusWXWeb.StartupTask;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace SchoolBusWXWeb
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateWebHostBuilder(args)
                .Build()
                .RunWithTasksAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
#if !DEBUG
                  .UseUrls("http://0.0.0.0:5005/")
#endif
                .UseStartup<Startup>().ConfigureLogging(builder => builder.AddFile());
    }
}
