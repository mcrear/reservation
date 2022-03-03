using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Reservation.Models.RequestModel
{
    // yeni bir oturum bilgisi almak için oluşturulan model
    public class SessionRequestModel
    {
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("connection")]
        public Connection Connection { get; set; }
        [JsonProperty("browser")]
        public Browser Browser { get; set; }
    }

    public class Browser
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("version")]
        public string Version { get; set; }
    }

    public class Connection
    {
        [JsonProperty("ip-address")]
        public string IpAddress { get; set; }
        [JsonProperty("port")]
        public string Port { get; set; }
    }
}
