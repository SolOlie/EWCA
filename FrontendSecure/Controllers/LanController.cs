using System;
using System.Collections.Generic;
using System.Linq;
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