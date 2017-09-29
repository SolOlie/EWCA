using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Entities.Entities;
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
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var name = "2doit@dk.dk";
            var user = new ApplicationUser();
            user.UserName = name;
            user.Email = name;
            string userPWD = "123456Dk";

            var result = userManager.Create(user, userPWD);

           
                var CRepo = new Facade().GetCustomerRepo();
                var URepo = new Facade().GetUserRepo();

                var c = CRepo.Read(1);
                if (c == null)
                {
                    c = new Customer()
                    {
                        Firm = "2doIT",
                        Address = "Esbjerg",
                        Date = DateTime.Now
                    };
                    c = CRepo.Create(c);
                }
               
                    var u = new User()
                    {
                        FirstName = "admin",
                        LastName = "istrator",
                        Email = name,
                        IsContactForCustomer = c,
                        Password = userPWD,
                        IsAdmin = true,
                        PhoneNumber = "*"

                    };
                    u = URepo.Create(u);

            

            ConfigureAuth(app);
        }
    }
}
