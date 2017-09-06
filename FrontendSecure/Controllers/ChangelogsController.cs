﻿using DevExpress.Web.Mvc;
using System;
using System.Net;
using System.Web.Mvc;
using Entities.Entities;
using FrontendSecure.Gateways;
using FrontendSecure.Models;

namespace FrontendSecure.Controllers
{
    public class ChangelogsController : Controller
    {
        private IServiceGateway<Changelog> db = new BllFacade().GetChangelogGateway();
        private IServiceGateway<Asset> adb = new BllFacade().GetAssetGateway();
        private IServiceGateway<File> fdb = new BllFacade().GetFileGateway();
        private IServiceGateway<User> udb = new BllFacade().GetUserGateway();
        
        [ValidateInput(false)]
        public ActionResult ChangelogTableExpressPartial(int assetid)
        {
            var model = new ChangelogsListPartialModel();
            model.Changelogs = adb.Read(assetid).Changelogs;
            model.assetid = assetid;
            return PartialView("~/Views/Customers/_ChangelogTableExpressPartial.cshtml", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ChangelogTableExpressPartialDelete(string Id)
        {
            Id = Id.Replace("\"", "");
            if (string.IsNullOrEmpty(Id))
            {
                Id = "0";
            }
            int id = Int32.Parse(Id);
            var model = new ChangelogsListPartialModel();
            if (id > 0)
            {
                try
                {
                    var a = db.Read(id);
                    db.Delete(a);
                    model.Changelogs = db.ReadAllWithFk(a.Asset.Id);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("~/Views/Customers/_ChangelogTableExpressPartial.cshtml", model);
        }


        [ValidateInput(false)]
        public ActionResult FileTableExpressPartial(int assetid)
        {
            var model = new FileListPartialModel();
            model.assetid = assetid;
            model.Files = adb.Read(assetid).FileAttachments;
            return PartialView("~/Views/Customers/_FileTableExpressPartial.cshtml", model);
        }

       
        [HttpPost, ValidateInput(false)]
        public ActionResult FileTableExpressPartialDelete(string Id)
        {
            var model = new FileListPartialModel();
            Id = Id.Replace("\"", "");
            if (string.IsNullOrEmpty(Id))
            {
                Id = "0";
            }
            int id = Int32.Parse(Id);
            if (id > 0)
            {
                try
                {
                    var c = fdb.Read(id);
                    fdb.Delete(c);
                    model.Files = adb.Read(c.Asset.Id).FileAttachments;
                    model.assetid = c.Asset.Id;
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("~/Views/Customers/_FileTableExpressPartial.cshtml", model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Changelog c = db.Read(id);
           

            if (c == null)
            {
                return HttpNotFound();
            }
            c.Asset.CustomerId = c.Asset.Customer.Id;

            return View(c);
        }
        [HttpPost]
        public ActionResult Edit(Changelog c)
        {
            c.UserId = c.User.Id;
            c.AssetId = c.Asset.Id;
            var isupdated = db.Update(c);
            

            if (!isupdated)
            {
                return View(c);
            }

            return RedirectToAction("AssetDetails", "Customers", new {id = c.Asset.Id, customerId = c.Asset.CustomerId});
        }

        [HttpGet]
        public ActionResult Create(int assetid)
        {
            Asset a =adb.Read(assetid);

            var users = udb.ReadAllWithFk(a.Customer.Id);
            if (a.Customer.Id != 1)
            {
            users.AddRange(udb.ReadAllWithFk(1));
            }

            var model = new CreateChangelogModel
            {
                Users = users,
                AssetId = assetid,
                Asset =a
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateChangelogModel ca, int AssetId)
        {
            Changelog c = ca.Changelog;
            c.Asset = new Asset
            {
                Id = AssetId
            };
            c.User = new User()
            {
                Id = c.UserId
            };
            var iscreated = db.Create(c);
            var a = adb.Read(AssetId);
            if (iscreated == null)
            {
               
                var users = udb.ReadAllWithFk(a.Customer.Id);
                if (a.Customer.Id != 1)
                {
                    users.AddRange(udb.ReadAllWithFk(1));
                }
                return View(new CreateChangelogModel {Users = users, AssetId = AssetId});

            }
            return RedirectToAction("AssetDetails", "Customers", new {id = a.Id, customerId = a.Customer.Id});
            
            }

        }
    }

