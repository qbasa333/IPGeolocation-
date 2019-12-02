using IpGeolocation.Database.Dao;
using IpGeolocation.Database.Models;
using IpGeolocation.Services.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IpGeolocation.Services.GeolocationSource
{
    public class IpStackService
    {
        protected static readonly string apiKey = System.Configuration.ConfigurationManager.AppSettings["IpStackApiKey"];
        protected static readonly string ipStackUrlFormat = $"http://api.ipstack.com/{{0}}?access_key={apiKey}";
        protected static readonly HttpClient client = new HttpClient() 
        { 
            Timeout = TimeSpan.FromSeconds(10), 
        };
        internal static async Task<string> GetIpStack(string ipOrHost)
        {
            HttpResponseMessage response = client.GetAsync(string.Format(ipStackUrlFormat, ipOrHost)).Result;
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            
            return await response.Content.ReadAsStringAsync();
        }
        internal static Geolocation GetGeolocationFromIpStack(IpStack ipStack, string ipOrUrl)
        {
            if (ipStack == null)
                return null;
            return new Geolocation
            {
                Ip = ipStack.Ip,
                Host = ipStack.Ip == ipOrUrl ? null : ipOrUrl,
                Latitude = ipStack.Latitude ?? 0,
                Longitude = ipStack.Longitude ?? 0,
                Location = $"{ipStack.CountryName}, {ipStack.City}"
            };

        }
        
    }
}
