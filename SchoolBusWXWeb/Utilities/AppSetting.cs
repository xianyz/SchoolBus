using Microsoft.Extensions.Configuration;
using SchoolBusWXWeb.Models;
using System.IO;

namespace SchoolBusWXWeb.Utilities
{
    public class AppSetting
    {
        private static readonly object ObjLock = new object(); //锁
        private static AppSetting _instance;

        //读取json文件
        private AppSetting()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);
            Config = builder.Build();
            siteConfig = Config.GetSection("Settings").Get<SiteConfig>();
        }

        private IConfigurationRoot Config { get; }
        private SiteConfig siteConfig { get; }

        /// <summary>
        ///     获取数据库字符串
        /// </summary>
        public static string DbConnection => GetConfig("SiteConfig:DefaultConnection");

        //加锁
        public static AppSetting GetInstance()
        {
            if (_instance == null)
            {
                lock (ObjLock)
                {
                    if (_instance == null) _instance = new AppSetting();
                }
            }
            return _instance;
        }

        /// <summary>
        ///     根据key获取value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetConfig(string name)
        {
            return GetInstance().Config.GetSection(name).Value;
        }

        /// <summary>
        ///     获取配置文件信息
        /// </summary>
        /// <returns></returns>
        public static SiteConfig GetConfig()
        {
            return GetInstance().siteConfig;
        }
    }
}
