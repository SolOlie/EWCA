using DevExpress.Web.Mvc;
using System;
using System.Net;
using System.Web.Mvc;
using Entities.Entities;
using FrontendSecure.Gateways;
using FrontendSecure.Models;

namespace FrontendSecure.Controllers
{
    //[Authorize]
    public class ChangelogsController : Controller
    {
        private IServiceGateway<Changelog> db = new BllFacade().GetChangelogGateway();
        private IServiceGateway<Asset> adb = new BllFacade().GetAssetGateway();
        private IServiceGateway<File> fdb = new BllFacade().GetFileGateway();
        private IServiceGateway<User> udb = new BllFacade().GetUserGateway();

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

        [ValidateInput(false)]
        public ActionResult ChangelogTableExpressPartial(int assetid)
        {
            var a = adb.Read(assetid);
            if (a == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (isAuthorized(a.Customer.Id) == AuthState.NoAuth)
            {
                return PartialView("NotAuthorizedPartical");
            }
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
                    var s = adb.Read(a.Asset.Id);
                    if (isAuthorized(s.Customer.Id) == AuthState.UserAuth || isAuthorized(s.Customer.Id) == AuthState.NoAuth)
                    {
                        return PartialView("NotAuthorizedPatialPopup");
                    }
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
            var asset = adb.Read(assetid);
            if (isAuthorized(asset.Customer.Id) == AuthState.NoAuth)
            {
                return View("NotAuthorized");
            }

            var model = new FileListPartialModel();
            model.assetid = assetid;
            model.Files = asset.FileAttachments;
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
                    var s = adb.Read(c.Asset.Id);
                    if (isAuthorized(s.Customer.Id) == AuthState.UserAuth || isAuthorized(s.Customer.Id) == AuthState.NoAuth)
                    {
                        return View("NotAuthorized");
                    }
                    fdb.Delete(c);
                    model.Files = s.FileAttachments;
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
            if (isAuthorized(c.Asset.Customer.Id) == AuthState.NoAuth)
            {
                return View("NotAuthorized");
            }
            c.Asset.CustomerId = c.Asset.Customer.Id;

            return View(c);
        }
        [HttpPost]
        public ActionResult Edit(Changelog c)
        {
            c.UserId = c.User.Id;
            c.AssetId = c.Asset.Id;
            if (!c.Asset.CustomerId.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (isAuthorized(c.Asset.CustomerId.Value) == AuthState.NoAuth)
            {
                return View("NotAuthorized");
            }
            var isupdated = db.Update(c);


            if (!isupdated)
            {
                return View(c);
            }

            return RedirectToAction("AssetDetails", "Customers", new { id = c.Asset.Id, customerId = c.Asset.CustomerId });
        }

        [HttpGet]
        public ActionResult Create(int assetid)
        {
            Asset a = adb.Read(assetid);

            var users = udb.ReadAllWithFk(a.Customer.Id);
            if (isAuthorized(a.Customer.Id) == AuthState.NoAuth)
            {
                return View("NotAuthorized");
            }
            if (a.Customer.Id != 1)
            {
                users.AddRange(udb.ReadAllWithFk(1));
            }

            var model = new CreateChangelogModel
            {
                Users = users,
                AssetId = assetid,
                Asset = a
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateChangelogModel ca, int AssetId)
        {
            var a = adb.Read(AssetId);
            Changelog c = ca.Changelog;
            if (isAuthorized(a.Customer.Id) == AuthState.NoAuth)
            {
                return View("NotAuthorized");
            }
            c.Asset = new Asset
            {
                Id = AssetId
            };
            c.User = udb.Read(c.UserId);
            var iscreated = db.Create(c);

            if (iscreated == null)
            {

                var users = udb.ReadAllWithFk(a.Customer.Id);
                if (a.Customer.Id != 1)
                {
                    users.AddRange(udb.ReadAllWithFk(1));
                }
                return View(new CreateChangelogModel { Users = users, AssetId = AssetId });

            }
            return RedirectToAction("AssetDetails", "Customers", new { id = a.Id, customerId = a.Customer.Id });

        }

    }
}

