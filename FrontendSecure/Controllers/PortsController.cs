using System;
using System.Web.Mvc;
using Entities.Entities;
using FrontendSecure.Gateways;
using FrontendSecure.Models;

namespace FrontendSecure.Controllers
{
    public class PortsController : Controller
    {
        private readonly IServiceGateway<Port> dbports = new BllFacade().GetPortGateway();
        private readonly IServiceGateway<Asset> dbAsset = new BllFacade().GetAssetGateway();

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

        public ActionResult PortListExpressPartialDelete(string Id)
        {
            Id = Id.Replace("\"", "");
            if (string.IsNullOrEmpty(Id))
            {
                Id = "0";
            }
            int id = Int32.Parse(Id);
            var model = new PortListPartialModel();
            if (id > 0)
            {
                try
                {
                    var c = dbports.Read(id);
                    dbports.Delete(c);
                    model.Ports = dbports.ReadAllWithFk(c.SwitchId);
                    model.assetid = c.SwitchId;
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("~/Views/Customers/_PortListExpressPartial.cshtml", model);
        }

        public ActionResult EditPort(int id, int assetid)
        {
            Port p = dbports.Read(id);
            int cid = dbAsset.Read(p.AssetId).Customer.Id;
            if (p.SwitchId == 0)
            {
                p.SwitchId = p.Switch.Id;
            }
            var model = new CreatePortModel()
            {
                Port = p,
                assetreturnid = assetid,
                customerId = cid,
                Assets = dbAsset.ReadAllWithFk(cid)
            };
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult EditPort(Port port, int customerid, int assetid)
        {
            var isupdated = dbports.Update(port);
            return RedirectToAction("AssetDetails", "Customers", new {id = assetid, customerId= customerid} );
        }

        public ActionResult CreatePort(int assetid)
        {
           int cid = dbAsset.Read(assetid).Customer.Id;
            var model = new CreatePortModel()
            {
                assetreturnid = assetid,
                Assets = dbAsset.ReadAllWithFk(cid),
                customerId = cid,
                Port = new Port() { SwitchId = assetid}
            };
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult CreatePort(Port port , int assetreturnid, int customerid)
        {
            if (ModelState.IsValid)
            {
                dbports.Create(port);
                return RedirectToAction("AssetDetails", "Customers", new { id = assetreturnid, customerId = customerid });
            }

            int cid = dbAsset.Read(assetreturnid).Customer.Id;
            var model = new CreatePortModel()
            {
                assetreturnid = assetreturnid,
                Assets = dbAsset.ReadAllWithFk(cid),
                customerId = cid
            };
            return View(model);
        }
    }
}