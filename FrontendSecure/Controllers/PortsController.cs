using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using DAL.DB;
using Entities.Entities;
using FrontendSecure.Gateways;
using FrontendSecure.Models;

namespace FrontendSecure.Controllers
{
    public class PortsController : Controller
    {
        private readonly IServiceGateway<Port> dbports = new BllFacade().GetPortGateway();

        [ValidateInput(false)]
        public ActionResult PortListExpressPartial(int assetid)
        {
            var model = new PortListPartialModel()
            {
                assetid = assetid,
                Ports = dbports.ReadAllWithFk(assetid)

            };
            return PartialView("~/Views/Customers/_PortListExpressPartial.cshtml", model);
        }

        [System.Web.Http.HttpPost, ValidateInput(false)]
      
        public ActionResult PortListExpressPartialDelete(System.Int32 Id)
        {
            var model = new PortListPartialModel();
            if (Id >= 0)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("~/Views/Customers/_PortListExpressPartial.cshtml", model);
        }
    }
}