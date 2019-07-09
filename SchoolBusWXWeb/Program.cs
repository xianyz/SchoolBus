using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using SchoolBusWXWeb.StartupTask;
using System.Threading.Tasks;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global


namespace SchoolBusWXWeb
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateWebHostBuilder(args)
                .Build()
                .RunAsync();
                //.RunWithTasksAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
#if !DEBUG
                  .UseUrls("http://0.0.0.0:5005/")
#endif
                .UseStartup<Startup>().ConfigureLogging(builder => builder.AddFile());
    }
}
