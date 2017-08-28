using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FrontendSecure.Models;
using Newtonsoft.Json;

namespace FrontendSecure.Controllers
{
    public class ApiController : Controller
    {
        protected void HandleBadRequest(ApiException apiException)
        {
            if (apiException.HttpStatusCode == HttpStatusCode.BadRequest)
            {
                var badRequestData = JsonConvert.DeserializeObject<JsonBadRequest>(apiException.JsonData);
                if (badRequestData.ModelState != null)
                {
                    foreach (var modelStateItem in badRequestData.ModelState)
                    {
                        foreach (var message in modelStateItem.Value)
                        {
                            ModelState.AddModelError(modelStateItem.Key,message);
                        }
                    }
                }
                if (string.Equals(badRequestData.Error, "invalid_grant"))
                {
                    ModelState.AddModelError("", badRequestData.ErrorDescription);
                }
            }
        }
    }
}