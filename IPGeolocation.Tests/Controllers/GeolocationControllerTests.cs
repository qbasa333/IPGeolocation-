using IpGeolocation.Controllers;
using IpGeolocation.Database.Dao;
using IpGeolocation.Database.Models;
using IpGeolocation.Tests.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace IpGeolocation.Tests.Controllers
{
    [TestClass]
    public class GeolocationControllerTests
    {
        private IpGeolocationController _controller = new IpGeolocationController()
        {
            Request = new HttpRequestMessage()
        };

        [TestInitialize]
        public void TestInit()
        {
            GeolocationDaoTests.CommonTestInit();
            _controller.Request.SetConfiguration(new HttpConfiguration());
        }

        [TestMethod]
        public void AddGeolocationTest()
        {
            var response = _controller.AddGeolocation("wp.pl");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("\"Geolocation added successfully\"", response.Content.ReadAsStringAsync().Result);
            var geolocaion = GeolocationDao.GetGeolocationByHost("wp.pl");
            Assert.AreEqual("wp.pl", geolocaion.Host);
        }

        [TestMethod]
        public void GetGeolocationTest()
        {
            var geolocation = new Geolocation()
            {
                Host = "host",
                Ip = "ip",
                Latitude = 0,
                Longitude = 0,
                Location = "location"

            };
            GeolocationDao.Insert(geolocation);
            var response = _controller.GetGeolocation(geolocation.Host);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var result = response.Content.ReadAsAsync<Geolocation>().Result;
            Assert.AreEqual(geolocation.Host, result.Host);
            Assert.AreEqual(geolocation.Ip, result.Ip);
            Assert.AreEqual(geolocation.Latitude, result.Latitude);
            Assert.AreEqual(geolocation.Longitude, result.Longitude);
            Assert.AreEqual(geolocation.Location, result.Location);
        }
        [TestMethod]
        public void DeleteGeolocationTest()
        {
            var geolocation = new Geolocation()
            {
                Host = "host",
                Ip = "ip2",
                Latitude = 0,
                Longitude = 0,
                Location = "location"

            };
            GeolocationDao.Insert(geolocation);
            var response = _controller.DeleteGeolocation(geolocation.Host);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsFalse(GeolocationDao.DeleteByHost(geolocation.Host));
        }
    }
}
