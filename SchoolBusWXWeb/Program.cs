using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SchoolBusWXWeb.StartupTask;
using System.Threading.Tasks;


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
                .UseStartup<Startup>();
    }
}
