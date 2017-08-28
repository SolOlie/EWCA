using System;
using System.Net;

namespace FrontendSecure
{
    public class ApiException : Exception
    {
        public ApiException(HttpStatusCode statusCode, string jsonData)
        {
            HttpStatusCode = statusCode;
            JsonData = jsonData;
        }

        public HttpStatusCode HttpStatusCode { get; set; }
        public string JsonData { get; set; }    
    }
}