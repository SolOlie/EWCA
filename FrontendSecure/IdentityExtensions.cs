using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace FrontendSecure
{
    public static class IdentityExtensions
    {
        public static int GetLoggedInUserId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.Name);
            // Test for null to avoid issues during local testing
            int i = 0;
            if (claim!=null)
            {
               var b = int.TryParse(claim.Value, out i);
            }
            return i;
        }
    }
}