using MQTTnet.Protocol;

namespace SchoolBusWXWeb.Models
{
    public class MqttMessageReceived
    {
        public string Topic { get;set;}
        public string Payload { get; set; }
        public MqttQualityOfServiceLevel QoS { get; set; }
        public bool Retain { get; set; }
    }
}
