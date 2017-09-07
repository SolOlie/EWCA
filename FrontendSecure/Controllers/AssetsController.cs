using System;
using DevExpress.Web.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Entities.Entities;
using FrontendSecure.Gateways;
using FrontendSecure.Models;
using TrackerEnabledDbContext.Common.Models;

namespace FrontendSecure.Controllers
{
    
    public class AssetsController : Controller
    {
        private IServiceGateway<Asset> db = new BllFacade().GetAssetGateway();
        private IServiceGateway<AssetType> dba = new BllFacade().GetAssetTypeGateway();
        private IServiceGateway<Customer>dbc = new BllFacade().GetCustomerGateway();
        private IServiceGateway<File>dbf = new BllFacade().GetFileGateway();
        private IServiceGateway<User>udb = new BllFacade().GetUserGateway();

        private enum AuthState
        {
            NoAuth,
            UserAuth,
            AdminAuth,
            ElitewebAuth
        };
        private AuthState isAuthorized(int customerId)
        {
            return AuthState.ElitewebAuth;
            var session = Session["loggedinUserId"];
            if (session == null)
            {
                return AuthState.NoAuth;
            }

            int loggedinUserId = (int)session;
            var loggedInUser = udb.Read(loggedinUserId);

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

        // GET: Assets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var asset = db.Read(id.Value);
            if (isAuthorized(asset.Customer.Id) == AuthState.NoAuth)
            {
                return View("NotAuthorized");
            }
            var model = new CreateAssetModel()
            {
                Asset = asset,
                AssetTypes = dba.ReadAll(),
                Customers = new List<Customer>() {dbc.Read(asset.Customer.Id)} 
            };
            if (model.Asset == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Assets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Asset asset)
        {
            
            if (isAuthorized(asset.Customer.Id) == AuthState.NoAuth)
            {
                return View("NotAuthorized");
            }
            if (ModelState.IsValid)
            {
               db.Update(asset);
                return RedirectToAction("Details", "Customers", new {id = asset.Customer.Id});
            }
            return View(new CreateAssetModel());
        }
        
        [HttpPost]
        public ActionResult AddFile(int assetId, HttpPostedFileBase upload)
        {
            var asset = db.Read(assetId);
            if (isAuthorized(asset.Customer.Id) == AuthState.NoAuth)
            {
                return View("NotAuthorized");
            }
            if (upload != null && upload.ContentLength > 0)
            {
                var attachment = new File()
                {
                    ContentType = upload.ContentType,
                    ContentFile = new ContentFile(),
                    Name = System.IO.Path.GetFileName(upload.FileName),
                };

                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    attachment.ContentFile.Content = reader.ReadBytes(upload.ContentLength);
                }
                attachment.AssetId = assetId;
                attachment.Asset = new Asset();
                attachment.Asset.Id = assetId;
                dbf.Create(attachment);
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        [ValidateInput(false)]
        public ActionResult AssetTableExpressPartial(int? customerid)
        {
            var model = new AssetListPartialModel();
            if (customerid.HasValue)
            {
                if (isAuthorized(customerid.Value) == AuthState.NoAuth)
                {
                    return PartialView("NotAuthorizedPartical");
                }
                model.Assets = db.ReadAllWithFk(customerid.Value);
                model.CustomerId = customerid.Value;
            }
            return PartialView("~/Views/Customers/_AssetTableExpressPartial.cshtml", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AssetTableExpressPartialDelete(string Id)
        {
            Id = Id.Replace("\"", "");
            if (string.IsNullOrEmpty(Id))
            {
                Id = "0";
            }
            int id = Int32.Parse(Id);
            var model = new AssetListPartialModel();
            if (id > 0)
            {
                try
                {
                   var a = db.Read(id);
                    if (isAuthorized(a.Customer.Id) == AuthState.NoAuth)
                    {
                        return PartialView("NotAuthorizedPatialPopup");
                    }
                    db.Delete(a);

                    model.Assets = db.ReadAllWithFk(a.Customer.Id);
                    model.CustomerId = a.Customer.Id;

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("~/Views/Customers/_AssetTableExpressPartial.cshtml", model);
        }
    }
}
