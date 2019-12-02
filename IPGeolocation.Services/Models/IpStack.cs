using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpGeolocation.Services.Models
{
    public class IpStack
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("country_name")]
        public string CountryName { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("latitude")]
        public double? Latitude { get; set; }

        [JsonProperty("longitude")]
        public double? Longitude { get; set; }
        public static IpStack FromJson(string json) => JsonConvert.DeserializeObject<IpStack>(json);
    }
}
