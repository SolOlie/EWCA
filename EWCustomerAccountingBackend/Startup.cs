using System;
using System.Collections.Generic;
using System.Linq;
using EWCustomerAccountingBackend.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(EWCustomerAccountingBackend.Startup))]

namespace EWCustomerAccountingBackend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var user = new ApplicationUser();
            user.UserName = "EliteWeb@dk.dk";
            user.Email = "EliteWeb@dk.dk";
            string userPWD = "EW1234";

           // var succes = UserManager.Create(user, userPWD);
            ConfigureAuth(app);
        }
    }
}
