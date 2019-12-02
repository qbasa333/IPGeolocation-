using IpGeolocation.Database.Models;
using IpGeolocation.Services;
using IpGeolocation.Services.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IpGeolocation.Controllers
{
    public class IpGeolocationController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage AddGeolocation([Url]string ipOrUrl)
        {
            if (!ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Input parameter ipOrUrl is not valid");
            var result = GeolocationService.AddGeolocation(ipOrUrl);
            switch (result)
            {
                case ResultCode.OK:
                    return Request.CreateResponse(HttpStatusCode.OK, "Geolocation added successfully");
                case ResultCode.ServiceUnavailable:
                    return Request.CreateResponse(HttpStatusCode.ServiceUnavailable, "IPStack service which is used for getting geolocation data is unavailable");
                case ResultCode.RecordAlreadyExists:
                    return Request.CreateResponse(HttpStatusCode.Conflict, "Geolocation with this ip or url is already added");
                case ResultCode.UrlNotFound:
                    return Request.CreateResponse((HttpStatusCode)422, "Geolocation with this ip or url cannot be found");
                case ResultCode.UnexpectedError:
                case ResultCode.DatabaseError:
                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetGeolocation(string ipOrUrl)
        {
            var result = GeolocationService.GetGeolocation(ipOrUrl);

            if (result.ResultCode == ResultCode.OK)
                return Request.CreateResponse(HttpStatusCode.OK, result.Geolocation);
            if (result.ResultCode == ResultCode.RecordDoesNotExist)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Geolocation is not found for this ip or url");
            return Request.CreateResponse(HttpStatusCode.InternalServerError);

        }
        [HttpDelete]
        public HttpResponseMessage DeleteGeolocation(string ipOrUrl)
        {
            var result = GeolocationService.DeleteGeolocation(ipOrUrl);
            if (result == ResultCode.OK)
                return Request.CreateResponse(HttpStatusCode.OK, "Geolocation removed successfully");
            if (result == ResultCode.RecordDoesNotExist)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Geolocation is not found for this ip or url");
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}
