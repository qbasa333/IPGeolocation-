using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpGeolocation.Database.Models
{
    public class Geolocation
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }
    }
}
