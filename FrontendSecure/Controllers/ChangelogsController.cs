using System;
using System.Net;
using System.Web.Mvc;
using Entities.Entities;
using FrontendSecure.Gateways;

namespace FrontendSecure.Controllers
{
    public class ChangelogsController : Controller
    {
        private IServiceGateway<Changelog> db = new BllFacade().GetChangelogGateway();

        // GET: Changelogs
        public ActionResult Index()
        {
            return View(db.ReadAll());
        }

        // GET: Changelogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Changelog Changelog = db.Read(id.Value);
            if (Changelog == null)
            {
                return HttpNotFound();
            }
            return View(Changelog);
        }

        // GET: Changelogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Changelogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Hours")] Changelog Changelog)
        {
            if (ModelState.IsValid)
            {
                db.Create(Changelog);
                return RedirectToAction("Index");
            }

            return View(Changelog);
        }

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

        // GET: Changelogs/Edit/5
        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Changelog Changelog = db.Read(Id.Value);
            if (Changelog == null)
            {
                return HttpNotFound();
            }
            return View(Changelog);
        }

        // POST: Changelogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Changelog Changelog)
        {
            if (ModelState.IsValid)
            {
                db.Update(Changelog);
                return RedirectToAction("AssetDetails", "Customers", new { id = Changelog.Id });
            }
            return View(Changelog);
        }

        // GET: Changelogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Changelog Changelog = db.Read(id.Value);
            if (Changelog == null)
            {
                return HttpNotFound();
            }
            return View(Changelog);
        }

        // POST: Changelogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Changelog changelog = db.Read(id);
            db.Delete(changelog);
            return RedirectToAction("Index");
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

    }
}
