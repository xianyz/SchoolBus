using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using SchoolBusWXWeb.Models;
using SchoolBusWXWeb.StartupTask;

// ReSharper disable StringLiteralTypo

namespace SchoolBusWXWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.Configure<SiteConfig>(Configuration.GetSection("SiteConfig"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddStartupTask<MqttStartupFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            #region 控制程序生命周期
            // 发生在应用启动成功以后，也就是Startup.Configure()方法结束后。
            //appLifetime.ApplicationStarted.Register(() =>
            //{
            //    Console.WriteLine("开始");
            //});
            //// 发生在程序正在执行退出的过程中，此时还有请求正在被处理。应用程序也会等到这个事件完成后，再退出。
            //appLifetime.ApplicationStopping.Register(() =>
            //{
            //    Console.WriteLine("关闭ing");
            //});
            //// 发生在程序正在完成正常退出的时候，所有请求都被处理完成。程序会在处理完这货的Action委托代码以后退出
            //appLifetime.ApplicationStopped.Register( () =>
            //{
            //    Console.WriteLine("关闭");
            //});
            #endregion
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
