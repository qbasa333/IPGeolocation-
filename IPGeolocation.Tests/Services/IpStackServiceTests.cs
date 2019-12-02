using IpGeolocation.Services.GeolocationSource;
using IpGeolocation.Services.Models;
using IpGeolocation.Tests.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpGeolocation.Tests.Services
{
    [TestClass]
    public class IpStackServiceTests
    {


        [TestMethod]
        public void GetIpStackTest()
        {
            var ipStack = IpStack.FromJson(IpStackService.GetIpStack("178.235.146.51").Result);
            Assert.AreEqual("Gdynia", ipStack.City);
            Assert.AreEqual("Poland", ipStack.CountryName);
            Assert.IsTrue(54.483871459960938 == ipStack.Latitude);
            Assert.IsTrue(18.464729309082031 == ipStack.Longitude);
            Assert.AreEqual("178.235.146.51", ipStack.Ip);
        }
    }
}
