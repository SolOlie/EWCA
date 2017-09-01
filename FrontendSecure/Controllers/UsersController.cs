using System;
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
    [HandleApiError]
    public class UsersController : ApiController
    {
        private IServiceGateway<User> db = new BllFacade().GetUserGateway();
        protected ApplicationDbContext applicationDbContext { get; set; }
        protected UserManager<ApplicationUser> userManager { get; set; }

        public UsersController()
        {
            this.applicationDbContext = new ApplicationDbContext();
            this.userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.applicationDbContext));
        }
        // GET: Users/Create
        public ActionResult Create(int? CustomerId)
        {
            if (CustomerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Model = new CreateUserWithCustomModel()
            {
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
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,FirstName,LastName,Password, PhoneNumber")] User user)
        {
            if (ModelState.IsValid)
            {
                User u = db.Read(user.Id);
                var model = new SetPasswordViewModel
                {
                    Email = u.Email,
                    Password = user.Password
                };
                var s = WebapiService.instance.PostAsync("/api/Account/SetPassword",model, System.Web.HttpContext.Current.User.Identity.Name).Result;
                user.IsContactForCustomer = u.IsContactForCustomer;
                user.Changelogs = u.Changelogs;
                user.Email = u.Email;
                
                db.Update(user);
                
                return RedirectToAction("Details", "Customers", new {id = u.IsContactForCustomer.Id});
            }
            return View(user);
        }
        [HttpPost]
        public bool AjaxCreateConfirmed(User users)
        {
            User user = db.Create(users);
            return true;
        }
        [HttpPost]
        public bool AjaxDeleteConfirmed(int id)
        {

            User user = db.Read(id);
            user.Password = "";
            var deleted = db.Update(user);
            if (deleted)
            {
                var s = WebapiService.instance.DeleteAsync<User>("/api/Account/RemoveLogin?email=" + user.Email, System.Web.HttpContext.Current.User.Identity.Name).Result;
                if (!s)
                {
                    return false;
                }
            }
            return true;
        }

        [ValidateInput(false)]
        public ActionResult UsersTableExpressPartial(int? customerid)
        {
            var model = new UsersListPartialModel();
            if (customerid.HasValue)
            {
                model.Users = db.ReadAllWithFk(customerid.Value);
                model.CustomerId = customerid.Value;
            }
            return PartialView("~/Views/Customers/_UsersTableExpressPartial.cshtml", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UsersTableExpressPartialDelete(System.Int32 Id)
        {
            var model = new UsersListPartialModel();
            if (Id > 0)
            {
                
                try
                {
                    var a = db.Read(Id);
                    db.Delete(a);
                    model.Users = db.ReadAllWithFk(a.IsContactForCustomer.Id);
                    model.CustomerId = a.IsContactForCustomer.Id;

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
