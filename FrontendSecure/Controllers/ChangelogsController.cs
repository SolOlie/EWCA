using DevExpress.Web.Mvc;
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
        
        [HttpPost]
        public bool ModifiedAdd(string Description, int userId,  double Hours, DateTime Date, int assetId, int? id)
        {
            if (Description == null)
            {
                return false;
            }
            db.Create(new Changelog()
            {
                Description = Description,
                User = new User()
                {
                  Id  = userId
                },
                Asset = new Asset()
                {
                    Id = assetId
                },
                Hours = Hours,
                ChangedDate = Date
            });
            return true;
        }
        [HttpPost]
        public bool ModifiedDelete(int id)
        {
            Changelog changelog = db.Read(id);
            var deleted = db.Delete(changelog);
            return deleted;
        }

        [HttpPost]
        public bool ModifiedEdit(string Description, int userId, double Hours, DateTime Date, int assetId, int id)
        {
            var changelog = db.Read(id);
            changelog.UserId = userId;
            changelog.User.Id = userId;
            changelog.Asset.Id = assetId;
            changelog.ChangedDate = Date;
            changelog.Description = Description;
            changelog.Hours = Hours;
            var updated = db.Update(changelog);
            return updated;
        }



        [ValidateInput(false)]
        public ActionResult ChangelogTableExpressPartial(int assetid)
        {
            var model = new ChangelogsListPartialModel();
            model.Changelogs = adb.Read(assetid).Changelogs;
            model.assetid = assetid;
            return PartialView("~/Views/Customers/_ChangelogTableExpressPartial.cshtml", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ChangelogTableExpressPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Entities.Entities.Changelog item)
        {
            var model = new ChangelogsListPartialModel();
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("~/Views/Customers/_ChangelogTableExpressPartial.cshtml", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ChangelogTableExpressPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Entities.Entities.Changelog item)
        {
            var model = new ChangelogsListPartialModel();
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
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
        public ActionResult FileTableExpressPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Entities.Entities.File item)
        {
            var model = new object[0];
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("~/Views/Customers/_FileTableExpressPartial.cshtml", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult FileTableExpressPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Entities.Entities.File item)
        {
            var model = new object[0];
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
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
    }
}
