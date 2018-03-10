using System;
using System.Net;

namespace Kralizek.Extensions.Http
{
    public class HttpException : Exception
    {
        public HttpException(HttpStatusCode status)
        {
            StatusCode = status;
        }

        public HttpStatusCode StatusCode { get; }

        public string ReasonPhrase { get; set; }
    }
}
