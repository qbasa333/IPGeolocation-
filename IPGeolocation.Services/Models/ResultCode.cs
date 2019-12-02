using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpGeolocation.Services.Models
{
    public enum ResultCode
    {
        OK,
        UrlNotFound,
        UnexpectedError,
        ServiceUnavailable,
        DatabaseError,
        RecordAlreadyExists,
        RecordDoesNotExist
    }
}
