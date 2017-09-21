using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Entities.Entities;
using FrontendSecure.Gateways;
using FrontendSecure.Gateways.SecureGateways;
using FrontendSecure.Models;

namespace FrontendSecure.Controllers
{
    public class FirewallController : Controller
    {
        IServiceGateway<Firewall> dbf = new BllFacade().GetFirewallGateway();
        IServiceGateway<Customer> dbc = new BllFacade().GetCustomerGateway();
        [ValidateInput(false)]
        public ActionResult FirewallTableExpressPartial(int Customerid)
        {
            var model = new FirewallListPartialModel()
            {
                customerid = Customerid,
                Firewalls = dbf.ReadAllWithFk(Customerid)
            };
            return PartialView("~/Views/Customers/_FirewallTableExpressPartial.cshtml", model);
        }
        
        [HttpPost, ValidateInput(false)]
        public ActionResult FirewallTableExpressPartialDelete(string Id)
        {
            Id = Id.Replace("\"", "");
            if (string.IsNullOrEmpty(Id))
            {
                Id = "0";
            }
            int id = Int32.Parse(Id);
            var model = new FirewallListPartialModel();
            if (id > 0)
            {
                try
                {
                    var d = dbf.Read(id);
                    dbf.Delete(d);
                    model.customerid = d.Customer.Id;
                    model.Firewalls = dbf.ReadAllWithFk(d.Customer.Id);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("~/Views/Customers/_FirewallTableExpressPartial.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(Firewall f)
        {
            if (!ModelState.IsValid)
            {
                return View(f);
            }
            f.Customer = null;
            dbf.Update(f);
            return RedirectToAction("Details", "Customers", new {id = f.CustomerId});
        }
        public ActionResult Edit(int id)
        {
            Firewall f = dbf.Read(id);
            f.CustomerId = f.Customer.Id;
            return View(f);
        }

        [HttpPost]
        public ActionResult Create(Firewall f)
        {
            if (!ModelState.IsValid)
            {
                return View(f);
            }
          var c = dbc.Read(f.CustomerId);
            f.Customer = c;
            dbf.Create(f);

            return RedirectToAction("Details", "Customers", new { id = f.CustomerId });
        }
        public ActionResult Create(int Customerid)
        {
            Firewall f = new Firewall();
            f.CustomerId = Customerid;
            return View(f);
        }
    }
}