using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using FrontendSecure;
using Microsoft.AspNet.Identity;

namespace EWCustomerAccountingBackend.Controllers
{
    public class GetLoggedinController : ApiController
    {

        public string Postloguser(IIdentity identity)
        {
            string i = HttpContext.Current.User.Identity.Name;
                return i; //identity.GetLoggedInUserId();

        }
    }
}
