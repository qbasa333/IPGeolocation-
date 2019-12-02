using IpGeolocation.Services;
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
    public class GeolocationServiceTests
    {
        [TestInitialize]
        public void TestInit()
        {
            GeolocationDaoTests.CommonTestInit();
        }

        [TestMethod]
        public void Test()
        {
            var result = GeolocationService.GetGeolocation("");
            Assert.AreEqual(ResultCode.RecordDoesNotExist, result.ResultCode);
            Assert.IsNull(result.Geolocation);

            result = GeolocationService.GetGeolocation("178.235.146.51");
            Assert.AreEqual(ResultCode.RecordDoesNotExist, result.ResultCode);
            Assert.IsNull(result.Geolocation);

            var resultCode = GeolocationService.AddGeolocation("178.235.146.51");
            Assert.AreEqual(ResultCode.OK, resultCode);

            result = GeolocationService.GetGeolocation("178.235.146.51");
            Assert.AreEqual(ResultCode.OK, result.ResultCode);
            Assert.IsNotNull(result.Geolocation);
            Assert.AreEqual("178.235.146.51", result.Geolocation.Ip);

            resultCode = GeolocationService.DeleteGeolocation("178.235.146.51");
            Assert.AreEqual(ResultCode.OK, resultCode);
        }
    }
}
