// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable ClassNeverInstantiated.Global
namespace SchoolBusWXWeb.Models
{
    public class SiteConfig
    {
        /// <summary>
        /// ScholleBus数据库链接字符串
        /// </summary>
        public string DefaultConnection { get; set; }
        
        /// <summary>
        /// 七牛AccessKey
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// 七牛SecretKey
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 七牛Bucket:first
        /// </summary>
        public string Bucketfirst { get; set; }

        /// <summary>
        /// 七牛Domain
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// mqtt配置项
        /// </summary>
        public MqttOption MqttOption { get; set; }
    }

    public class MqttOption
    {
        public string HostIp { get; set; }
        public int HostPort { get; set; }
        public int Timeout { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ClientID { get; set; }
        public string MqttTopic { get; set; }
    }
}
