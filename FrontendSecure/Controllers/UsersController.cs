﻿using System.Net;
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
        // GET: Users
        public ActionResult Index()
        {
            return View(db.ReadAll());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
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
                db.Update(user);
                User u = db.Read(user.Id);
                return RedirectToAction("Details", "Customers", new {id = u.IsContactForCustomer.Id});
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Read(id);
            db.Delete(user);
            return RedirectToAction("Index");
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

            db.Delete(user);
            return true;
        }
    }
}
