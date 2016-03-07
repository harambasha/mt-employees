using System;
using System.Net;

namespace MT.WebAPI.HttpPipeline
{
    public class ApiException : Exception
    {
      public ApiException(string message)
            : this(HttpStatusCode.InternalServerError, message)
        {
        }

        public ApiException(HttpStatusCode statusCode)
            : this(statusCode, statusCode.ToString())
        {
        }

        public ApiException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}