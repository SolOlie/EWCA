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
        

        // GET: Assets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Asset asset = db.Read(id.Value);
            
            if (asset == null)
            {
                return HttpNotFound();
            }
           
            
            return View(asset);
        }
        // GET: Assets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var asset = db.Read(id.Value);
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
            if (ModelState.IsValid)
            {
               db.Update(asset);
                return RedirectToAction("Details", "Customers", new {id = asset.Customer.Id});
            }
            return View(new CreateAssetModel());
        }
        [HttpPost]
        public bool ModifiedDelete(int id) 
        {
            Asset asset = db.Read(id);
            return db.Delete(asset);
        }//bool til at checke om der bliver returneret true eller false når vi sletter en asset i vores liste.
        [HttpPost]
        public ActionResult AddFile(int assetId, HttpPostedFileBase upload)
        {
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

        [HttpPost]
        public bool ModifiedDeleteFile(int id)
        {
            var file = dbf.Read(id);
            var deleted = dbf.Delete(file);
            return deleted;
        }


        [ValidateInput(false)]
        public ActionResult AssetTableExpressPartial(int? customerid)
        {
            var model = new AssetListPartialModel();
            if (customerid.HasValue)
            {
                model.Assets = db.ReadAllWithFk(customerid.Value);
                model.CustomerId = customerid.Value;
            }
            return PartialView("~/Views/Customers/_AssetTableExpressPartial.cshtml", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AssetTableExpressPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Entities.Entities.Asset item)
        {
            var model = new AssetListPartialModel();
            if (ModelState.IsValid)
            {
                try
                {
                  
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("~/Views/Customers/_AssetTableExpressPartial.cshtml", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult AssetTableExpressPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Entities.Entities.Asset item)
        {
            var model = new AssetListPartialModel();
            if (ModelState.IsValid)
            {
                try
                {
                    var a = db.Read(item.Id);
                    item.Customer = a.Customer;
                    item.CustomerId = a.Customer.Id;
                    item.Changelogs = a.Changelogs;
                    item.FileAttachments = a.FileAttachments;
                    item.InstallationDate = a.InstallationDate;
                    item.Type = a.Type;
                    item.TypeId = a.Type.Id;
                    var b = db.Update(item);
                    if (b)
                    {
                        
                        model.CustomerId = a.Customer.Id;
                        model.Assets = db.ReadAllWithFk(a.Customer.Id);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
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
