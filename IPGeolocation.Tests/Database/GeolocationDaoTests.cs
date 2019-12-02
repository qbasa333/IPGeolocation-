using IpGeolocation.Database.Dao;
using IpGeolocation.Database.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpGeolocation.Tests.Database
{
    [TestClass]
    public class GeolocationDaoTests
    {
        [TestInitialize]
        public void TestInit()
        {
            CommonTestInit();
        }
        public static void CommonTestInit()
        {
            using (var connection = new SQLiteConnection(GeolocationDao.conectionString))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand("DELETE FROM Geolocations", connection))
                {
                    var result = cmd.ExecuteScalar();

                }
            }
        }

        [TestMethod]
        public void Test()
        {
            var testGeolocation = new Geolocation()
            {
                Ip = "ip",
                Latitude = 1,
                Longitude = 2,
                Location = "secret test location"
            };
            var result = GeolocationDao.Insert(testGeolocation);
            Assert.IsTrue(result);
            var geolocationResult = GeolocationDao.GetGeolocationByIp("ip");

            Assert.AreEqual(testGeolocation.Ip, geolocationResult.Ip);
            Assert.AreEqual(testGeolocation.Host, geolocationResult.Host);
            Assert.AreEqual(testGeolocation.Latitude, geolocationResult.Latitude);
            Assert.AreEqual(testGeolocation.Longitude, geolocationResult.Longitude);
            Assert.AreEqual(testGeolocation.Location, geolocationResult.Location);

            Assert.IsNull(GeolocationDao.GetGeolocationByIpOrHost(null));
            Assert.IsNull(GeolocationDao.GetGeolocationByIpOrHost(""));
            Assert.IsNull(GeolocationDao.GetGeolocationByIpOrHost("ip2"));

            Assert.IsFalse(GeolocationDao.DeleteByHost(null));
            Assert.IsFalse(GeolocationDao.DeleteByHost(""));
            Assert.IsFalse(GeolocationDao.DeleteByHost("ip"));
            Assert.IsFalse(GeolocationDao.DeleteByIp(null));
            Assert.IsFalse(GeolocationDao.DeleteByIp(""));
            Assert.IsFalse(GeolocationDao.DeleteByIp("test ip"));
            Assert.IsTrue(GeolocationDao.DeleteByIp("ip"));

            Assert.IsNull(GeolocationDao.GetGeolocationByIp("ip"));

            testGeolocation = new Geolocation()
            {
                Ip = "ip",
                Host = "host",
                Latitude = 1,
                Longitude = 2,
                Location = "secret test location"
            };
            result = GeolocationDao.Insert(testGeolocation);
            Assert.IsTrue(result);

            Assert.IsNotNull(GeolocationDao.GetGeolocationByIpOrHost("host"));
            Assert.IsTrue(GeolocationDao.DeleteByHost("host"));
            Assert.IsNull(GeolocationDao.GetGeolocationByIpOrHost("host"));

        }
    }
}
