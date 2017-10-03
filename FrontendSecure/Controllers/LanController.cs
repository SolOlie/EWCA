using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Entities.Entities;
using FrontendSecure.Gateways;
using FrontendSecure.Models;

namespace FrontendSecure.Controllers
{
    [Authorize]
    public class LanController : Controller
    {
         private readonly IServiceGateway<Lan> dbl = new BllFacade().GetLanGateway();
       private readonly IServiceGateway<Customer> dbc = new BllFacade().GetCustomerGateway();

        private readonly IServiceGateway<User> db = new BllFacade().GetUserGateway();
        private enum AuthStates
        {
            NoAuth,
            UserAuth,
            AdminAuth,
            ElitewebAuth
        };
        private AuthStates isAuthorized(int customerId)
        {
            // return AuthStates.ElitewebAuth;
            HttpCookie myCookie = Request.Cookies["UserCookie"];
            if (myCookie == null)
            {
                return AuthStates.NoAuth;
            }
            int i = 0;
            //ok - cookie is found.
            //Gracefully check if the cookie has the key-value as expected.
            if (!string.IsNullOrEmpty(myCookie.Values["userid"]))
            {
                string userId = myCookie.Values["userid"].ToString();
                int.TryParse(userId, out i);

                //Yes userId is found. Mission accomplished.
            }

            int loggedinUserId = i;
            var loggedInUser = db.Read(loggedinUserId);

            if (loggedInUser == null)
            {
                return AuthStates.NoAuth;
            }

            if (loggedInUser.IsContactForCustomer.Id > 0)
            {
                if (1 == loggedInUser.IsContactForCustomer.Id)
                {
                    return AuthStates.ElitewebAuth;
                }
                if (customerId == loggedInUser.IsContactForCustomer.Id)
                {
                    if (loggedInUser.IsAdmin)
                    {
                        return AuthStates.AdminAuth;
                    }
                    return AuthStates.UserAuth;
                }

                return AuthStates.NoAuth;

            }
            return AuthStates.NoAuth;
        }
        [ValidateInput(false)]
        public ActionResult LanTableExpressPartial(int Customerid)
        {
            if (isAuthorized(Customerid) == AuthStates.NoAuth)
            {
                return PartialView("NotAuthorizedPartical");
            }
            var model =  new LanListPartialModel()
            {
                customerid = Customerid,
                Lans = dbl.ReadAllWithFk(Customerid)
            };
            return PartialView("~/Views/Customers/_LanTableExpressPartial.cshtml", model);
        }
        
        [HttpPost, ValidateInput(false)]
        public ActionResult LanTableExpressPartialDelete(string Id)
        {
            Id = Id.Replace("\"", "");
            if (string.IsNullOrEmpty(Id))
            {
                Id = "0";
            }
            int id = Int32.Parse(Id);
            var model = new LanListPartialModel();
            if (id > 0)
            {
                try
                {
                    var d = dbl.Read(id);
                    if (isAuthorized(d.Customer.Id) == AuthStates.NoAuth)
                    {
                        return PartialView("NotAuthorizedPartical");
                    }
                    dbl.Delete(d);
                    model.customerid = d.Customer.Id;
                    model.Lans = dbl.ReadAllWithFk(d.Customer.Id);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("~/Views/Customers/_LanTableExpressPartial.cshtml", model);
        }
        [HttpPost]
        public ActionResult Edit(Lan f)
        {
            if (isAuthorized(f.CustomerId) == AuthStates.NoAuth)
            {
                return View("NotAuthorized");
            }
            if (!ModelState.IsValid)
            {
                return View(f);
            }
            f.Customer = null;
            dbl.Update(f);
            return RedirectToAction("Details", "Customers", new { id = f.CustomerId });
        }
        public ActionResult Edit(int id)
        {
            Lan f = dbl.Read(id);
            f.CustomerId = f.Customer.Id;
            if (isAuthorized(f.CustomerId) == AuthStates.NoAuth)
            {
                return View("NotAuthorized");
            }
            return View(f);
        }

        [HttpPost]
        public ActionResult Create(Lan f)
        {
            if (!ModelState.IsValid)
            {
                return View(f);
            }
            var c = dbc.Read(f.CustomerId);
            if (isAuthorized(c.Id) == AuthStates.NoAuth)
            {
                return View("NotAuthorized");
            }
            f.Customer = c;
            dbl.Create(f);

            return RedirectToAction("Details", "Customers", new { id = f.CustomerId });
        }
        public ActionResult Create(int Customerid)
        {
            if (isAuthorized(Customerid) == AuthStates.NoAuth)
            {
                return View("NotAuthorized");
            }
            Lan f = new Lan();
            f.CustomerId = Customerid;

            return View(f);
        }

    }
}