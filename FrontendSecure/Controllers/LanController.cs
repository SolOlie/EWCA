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
    public class LanController : Controller
    {
        IServiceGateway<Lan> dbl = new BllFacade().GetLanGateway();
        IServiceGateway<Customer> dbc = new BllFacade().GetCustomerGateway();
        [ValidateInput(false)]
        public ActionResult LanTableExpressPartial(int Customerid)
        {
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
            f.Customer = c;
            dbl.Create(f);

            return RedirectToAction("Details", "Customers", new { id = f.CustomerId });
        }
        public ActionResult Create(int Customerid)
        {
            Lan f = new Lan();
            f.CustomerId = Customerid;
            return View(f);
        }

    }
}