using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolBusWXWeb.Models
{
    public class SiteConfig
    {
        public MqttOption mqttOption { get; set; }
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
