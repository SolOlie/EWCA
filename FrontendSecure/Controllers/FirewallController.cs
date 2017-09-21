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

        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
    }
}