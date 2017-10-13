using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities.Entities;
using FrontendSecure.Gateways;
using FrontendSecure.Models;

namespace FrontendSecure.Controllers
{
    public class CustomerFileController : Controller
    {
        private readonly IServiceGateway<CustomerFile> dbFile = new BllFacade().GetCustomerFileGateway();
        private readonly IServiceGateway<Customer> db= new BllFacade().GetCustomerGateway();
        private readonly IServiceGateway<User> dbUser = new BllFacade().GetUserGateway();
        private enum AuthState
        {
            NoAuth,
            UserAuth,
            AdminAuth,
            ElitewebAuth
        };
        private AuthState isAuthorized(int customerId)
        {
            // return AuthState.ElitewebAuth;
            HttpCookie myCookie = Request.Cookies["UserCookie"];
            if (myCookie == null)
            {
                return AuthState.NoAuth;
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
            var loggedInUser = dbUser.Read(loggedinUserId);

            if (loggedInUser == null)
            {
                return AuthState.NoAuth;
            }

            if (loggedInUser.IsContactForCustomer.Id > 0)
            {
                if (1 == loggedInUser.IsContactForCustomer.Id)
                {
                    return AuthState.ElitewebAuth;
                }
                if (customerId == loggedInUser.IsContactForCustomer.Id)
                {
                    if (loggedInUser.IsAdmin)
                    {
                        return AuthState.AdminAuth;
                    }
                    return AuthState.UserAuth;
                }

                return AuthState.NoAuth;

            }
            return AuthState.NoAuth;
        }
        // GET: CustomerFile
      
        [HttpPost]
        public ActionResult AddFile(int customerId, HttpPostedFileBase upload)
        {
            var customer = db.Read(customerId);
            if (isAuthorized(customer.Id) == AuthState.NoAuth)
            {
                return View("NotAuthorized");
            }
            if (upload != null && upload.ContentLength > 0)
            {
                var attachment = new CustomerFile()
                {
                    ContentType = upload.ContentType,
                    CustomerContentType = new CustomerContentType(),
                    Name = System.IO.Path.GetFileName(upload.FileName),
                };

                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    attachment.CustomerContentType.Content = reader.ReadBytes(upload.ContentLength);
                }
                attachment.CustomerId = customerId;
                attachment.Customer = new Customer();
                attachment.Customer.Id = customerId;
                dbFile.Create(attachment);
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpGet]
        public FileResult Download(int Id)
        {

            var file = dbFile.Read(Id);
            return File(file.CustomerContentType.Content, file.ContentType, file.Name);
        }
        [ValidateInput(false)]
        public ActionResult CustomerFileListExpressPartial(int? customerid)
        {
            var model = new CustomerFileList();
            if (customerid.HasValue)
            {
                if (isAuthorized(1) != AuthState.ElitewebAuth)
                {
                    return PartialView("NotAuthorized");
                }
                model.CustomerFiles = dbFile.ReadAllWithFk(customerid.Value);
                model.CustomerId = customerid.Value;
            }
            return PartialView("~/Views/Customers/_CustomerFileListExpressPartial.cshtml", model);

        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CustomerFileListExpressPartialDelete(string Id)
        {
            Id = Id.Replace("\"", "");
            if (string.IsNullOrEmpty(Id))
            {
                Id = "0";
            }
            int id = Int32.Parse(Id);
            var model = new CustomerFileList();
            if (id > 0)
            {
                try
                {
                    var d = dbFile.Read(id);
                    
                    dbFile.Delete(d);
                    model.CustomerId = d.Customer.Id;
                    model.CustomerFiles = dbFile.ReadAllWithFk(d.Customer.Id);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("~/Views/Customers/_CustomerFileListExpressPartial.cshtml", model);
        }
    }
}