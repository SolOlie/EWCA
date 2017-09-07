using System;
using System.Linq;
using DevExpress.Web.Mvc;
using System.Net;
using System.Web.Mvc;
using Entities.Entities;
using FrontendSecure.Filters;
using FrontendSecure.Gateways;
using FrontendSecure.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FrontendSecure.Controllers
{
    [Authorize]
    [HandleApiError]
    public class UsersController : ApiController
    {
        private IServiceGateway<User> db = new BllFacade().GetUserGateway();
        private enum AuthStates
        {
            NoAuth,
            UserAuth,
            AdminAuth,
            ElitewebAuth
        };
        private AuthStates isAuthorized(int customerId)
        {
            //return AuthStates.ElitewebAuth;
            var session = Session["loggedinUserId"];
            if (session == null)
            {
                return AuthStates.NoAuth;
            }

            int loggedinUserId = (int)session;
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
        // GET: Users/Create
        public ActionResult Create(int? CustomerId)
        {

            if (CustomerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (isAuthorized(CustomerId.Value) == AuthStates.UserAuth || isAuthorized(CustomerId.Value) == AuthStates.NoAuth)
            {
                return View("NotAuthorized");
            }

            var Model = new CreateUserWithCustomModel()
            {
                Users = db.ReadAll().Select(e => e.Email).ToList(),
                CustomerId = CustomerId.Value,
                User = new User()
            };
            return View(Model);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user, int CustomerId)
        {
            if (isAuthorized(CustomerId) == AuthStates.UserAuth || isAuthorized(CustomerId) == AuthStates.NoAuth)
            {
                return View("NotAuthorized");
            }
            if (ModelState.IsValid)
            {
                user.IsContactForCustomer = new Customer()
                {
                    Id = CustomerId
                };

                var modelregister = new RegisterViewModel()
                {
                    Password = user.Password,
                    ConfirmPassword = user.Password,
                    Email = user.Email
                };
                var model = new CreateUserWithCustomModel()
                {
                    CustomerId = CustomerId,
                    User = new User()
                };
                try
                {
                    var s = WebapiService.instance.PostAsync("/api/Account/Register", modelregister).Result;
                    var u = db.Create(user);
                    return RedirectToAction("Details", "Customers", new { id = CustomerId });
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
            var Model = new CreateUserWithCustomModel()
            {
                CustomerId = CustomerId,
                User = new User()
            };
            return View(Model);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Read(id.Value);

            if (user == null)
            {
                return HttpNotFound();
            }
            if (isAuthorized(user.IsContactForCustomer.Id) == AuthStates.UserAuth || isAuthorized(user.IsContactForCustomer.Id) == AuthStates.NoAuth)
            {
                return View("NotAuthorized");
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,FirstName,LastName,Password, PhoneNumber, IsAdmin")] User user)
        {
            User u = db.Read(user.Id);
            if (isAuthorized(u.IsContactForCustomer.Id) == AuthStates.UserAuth || isAuthorized(u.IsContactForCustomer.Id) == AuthStates.NoAuth)
            {
                return View("NotAuthorized");
            }
            user.IsContactForCustomer = u.IsContactForCustomer;
            user.Email = u.Email;
            if (ModelState.IsValid)
            {

                var model = new SetPasswordViewModel
                {
                    Email = u.Email,
                    Password = user.Password
                };
                var s = WebapiService.instance.PostAsync("/api/Account/SetPassword", model, System.Web.HttpContext.Current.User.Identity.Name).Result;
                user.IsContactForCustomer = u.IsContactForCustomer;
                user.Changelogs = u.Changelogs;
                user.Email = u.Email;

                db.Update(user);

                return RedirectToAction("Details", "Customers", new { id = u.IsContactForCustomer.Id });
            }
            return View(user);
        }

        [ValidateInput(false)]
        public ActionResult UsersTableExpressPartial(int? customerid)
        {

            var model = new UsersListPartialModel();
            if (customerid.HasValue)
            {
                if (isAuthorized(customerid.Value) == AuthStates.UserAuth || isAuthorized(customerid.Value) == AuthStates.NoAuth)
                {
                    return PartialView("NotAuthorizedPartical");
                }
                model.Users = db.ReadAllWithFk(customerid.Value);
                model.CustomerId = customerid.Value;
            }
            return PartialView("~/Views/Customers/_UsersTableExpressPartial.cshtml", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UsersTableExpressPartialDelete(string Id)
        {
            Id = Id.Replace("\"", "");
            if (string.IsNullOrEmpty(Id))
            {
                Id = "0";
            }
            int id = Int32.Parse(Id);
            var model = new UsersListPartialModel();
            if (id > 0)
            {

                try
                {
                    User user = db.Read(id);
                    if (isAuthorized(user.IsContactForCustomer.Id) == AuthStates.UserAuth || isAuthorized(user.IsContactForCustomer.Id) == AuthStates.NoAuth)
                    {
                        return PartialView("NotAuthorizedPartical");
                    }
                    user.Password = "";
                    var deleted = db.Update(user);
                    if (deleted)
                    {
                        var s =
                            WebapiService.instance.DeleteAsync<User>("/api/Account/RemoveLogin?email=" + user.Email,
                                System.Web.HttpContext.Current.User.Identity.Name).Result;
                    }

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("~/Views/Customers/_UsersTableExpressPartial.cshtml", model);
        }
    }
}
