using IpGeolocation.Database.Dao;
using IpGeolocation.Database.Models;
using IpGeolocation.Services.GeolocationSource;
using IpGeolocation.Services.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpGeolocation.Services
{
    public class GeolocationService
    {
        private static readonly  ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public struct GeolocationResult
        {
            public Geolocation Geolocation;
            public ResultCode ResultCode;
        }
        public static ResultCode AddGeolocation(string ipOrUrl)
        {
            if (GeolocationDao.GetGeolocationByIpOrHost(ipOrUrl) != null)
                return ResultCode.RecordAlreadyExists;
            string response = null;
            try
            {
                response = IpStackService.GetIpStack(ipOrUrl).Result;
            }
            catch (Exception e)
            {
                log.Error("Failed to get geolocation from IPStack", e);
                return ResultCode.ServiceUnavailable;
            }
            if (response == null)
                return ResultCode.ServiceUnavailable;
            IpStack ipStack = null;
            try
            {
                ipStack = IpStack.FromJson(response);
            }
            catch (Exception e)
            {
                log.Error("Failed to parse json from IPStack", e);
                return ResultCode.UnexpectedError;
            }
            if (ipStack.Latitude == null || ipStack.Longitude == null)
                return ResultCode.UrlNotFound;
            var geolocation = IpStackService.GetGeolocationFromIpStack(ipStack, ipOrUrl);

            try
            {
                if (GeolocationDao.Insert(geolocation))
                    return ResultCode.OK;
            }
            catch (Exception)
            {
                return ResultCode.DatabaseError;
            }
            return ResultCode.UnexpectedError;
        }

        public static GeolocationResult GetGeolocation(string ipOrUrl)
        {
            try
            {
                var geolocation = GeolocationDao.GetGeolocationByIpOrHost(ipOrUrl);
                return new GeolocationResult()
                {
                    Geolocation = geolocation,
                    ResultCode = geolocation == null ? ResultCode.RecordDoesNotExist : ResultCode.OK
                };
            }
            catch (Exception)
            {
                return new GeolocationResult() { ResultCode = ResultCode.DatabaseError };
            }
        }
        public static ResultCode DeleteGeolocation(string ipOrUrl)
        {
            try
            {
                return GeolocationDao.DeleteByIpOrHost(ipOrUrl) ? ResultCode.OK : ResultCode.RecordDoesNotExist;
            }
            catch (Exception)
            {
                return ResultCode.DatabaseError;
            }
        }
    }
}
