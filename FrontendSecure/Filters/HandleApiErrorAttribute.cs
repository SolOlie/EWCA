using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace FrontendSecure.Filters
{
    public class HandleApiErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var apiException = filterContext.Exception as ApiException;
            if (!filterContext.ExceptionHandled && apiException != null)
            {
                switch (apiException.HttpStatusCode)
                {
                        case HttpStatusCode.InternalServerError:
                        var errorDetails = JsonConvert.DeserializeObject<JsonExceptionMessage>(apiException.JsonData);
                        filterContext.Exception = new Exception(errorDetails.ExceptionMessage);
                        break;
                }
            }
            base.OnException(filterContext);
        }
    private class JsonExceptionMessage
    {
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
    }
    }

}