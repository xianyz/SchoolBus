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
        /// 七牛配置项
        /// </summary>
        public QiniuOption QiniuOption { get; set; }

        /// <summary>
        /// mqtt配置项
        /// </summary>
        public MqttOption MqttOption { get; set; }

        /// <summary>
        /// 阿里云配置
        /// </summary>
        public AliOption AliOption { get; set; }
    }
    public class QiniuOption
    {
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
    public class AliOption
    {
        public string RegionId { get;set;}
        public string Product { get; set; }
        public string Domain { get; set; }
        public string Action { get; set; }
        public string AccessKeyId { get; set; }
        public string AccessKeySecret { get; set; }
        public string SignName { get; set; }
        public string TemplateCode { get; set; }
        public string OutId { get; set; }
        public string Version { get;set;}
    }
}
