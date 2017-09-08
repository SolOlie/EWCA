using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities.Entities;
using FrontendSecure.Gateways;
using FrontendSecure.Gateways.SecureGateways;
using FrontendSecure.Models;

namespace FrontendSecure.Controllers
{
    public class SwitchController : Controller
    {

        private IServiceGateway<Switch> dbs = new BllFacade().GetSwitchGateway();
        private IServiceGateway<Asset> dba = new BllFacade().GetAssetGateway();
        [ValidateInput(false)]
        public ActionResult SwitchTableExpressPartial(int customerid)
        {
            var model = new SwitchListPartialModel()
            {
                CustomerId = customerid,
                Switches = dbs.ReadAllWithFk(customerid)
            };
            return PartialView("~/Views/Customers/_SwitchTableExpressPartial.cshtml", model);
        }
        
        [HttpPost, ValidateInput(false)]
        public ActionResult SwitchTableExpressPartialDelete(string Id)
        {
            Id = Id.Replace("\"", "");
            if (string.IsNullOrEmpty(Id))
            {
                Id = "0";
            }
            int id = Int32.Parse(Id);
            var model = new SwitchListPartialModel();
            if (id > 0)
            {
                try
                {
                    var a = dbs.Read(id);
                    dbs.Delete(a);
                    model.Switches = dbs.ReadAllWithFk(a.Customer.Id);
                    model.CustomerId = a.Customer.Id;
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("~/Views/Customers/_SwitchTableExpressPartial.cshtml", model);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            throw new NotImplementedException();
        }

        public ActionResult Details(int id, int customerid)
        {
            throw new NotImplementedException();
        }
       }
}