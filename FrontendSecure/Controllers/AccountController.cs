using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FrontendSecure.Filters;
using FrontendSecure.Gateways;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using FrontendSecure.Models;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.DataHandler.Serializer;

namespace FrontendSecure.Controllers
{
    [HandleApiError]
    public class AccountController : ApiController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult SignIn()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> SignIn(SignInModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await WebapiService.instance.AuthedicateAsync<SignInResult>(model.Email, model.Password);
                FormsAuthentication.SetAuthCookie(result.AccessToken, model.RememberMe);
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, result.UserName),
                    new Claim(ClaimTypes.NameIdentifier, result.UserName),
                };
                var authTicket = new AuthenticationTicket(new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie), new AuthenticationProperties
                {
                    ExpiresUtc = result.Expires,
                    IsPersistent = model.RememberMe,
                    IssuedUtc = result.Issued,
                    RedirectUri = returnUrl
                } );
                byte[] userData = DataSerializers.Ticket.Serialize(authTicket);
                byte[] protectedData = MachineKey.Protect(userData, new[]
                {
                    "Microsoft.Owin.Security.Cookies.CookieAuthenticationMiddleware",
                    DefaultAuthenticationTypes.ApplicationCookie, "v1"
                });
                string protectedText = TextEncodings.Base64Url.Encode(protectedData);
                Response.SetCookie(new HttpCookie("EWCA.webapi.Auth")
                {
                    HttpOnly = true,
                    Expires = result.Expires.UtcDateTime,
                    Value = protectedText
                });
                var dbUsers = new BllFacade().GetUserGateway();
                var users = dbUsers.ReadAll().Find(x => x.Email == model.Email);
                if (users != null)
                {
                    if (users.Password == model.Password)
                    {
                        HttpCookie myCookie = new HttpCookie("UserCookie");
                        
                        //Add key-values in the cookie
                        myCookie.Values.Add("userid", users.Id.ToString());
                        myCookie.HttpOnly = true;
                        //set cookie expiry date-time. Made it to last for next 12 hours.
                        myCookie.Expires = DateTime.Now.AddHours(12).ToUniversalTime();

                        //Most important, write the cookie to client.
                        Response.Cookies.Add(myCookie);

                        //Session["loggedinUserId"] = users.Id;
                    }
                    if (users.IsContactForCustomer.Id != 1)
                    {
                        return RedirectToAction("Details", "Customers", new {users.IsContactForCustomer.Id});
                    }
                }
                return Redirect(returnUrl??"/");
            }
            catch (ApiException ex)
            {
                HandleBadRequest(ex);
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                throw;
            }
        }
        [Authorize]
        public ActionResult SingOut()
        {
            FormsAuthentication.SignOut();
            if (Response.Cookies["EWCA.webapi.Auth"] != null)
            {
                var c = new HttpCookie("EWCA.webapi.Auth")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(c);
            }

            if (Response.Cookies["UserCookie"] != null)
            {
                var c = new HttpCookie("UserCookie")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(c);
            }
            return RedirectToAction("SignIn", "Account");
        }
    }
}