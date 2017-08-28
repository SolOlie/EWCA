using System.Net;
using System.Web.Mvc;
using Entities.Entities;
using FrontendSecure.Gateways;

namespace FrontendSecure.Controllers
{
    public class AssetTypesController : Controller
    {
        private IServiceGateway<AssetType> db = new BllFacade().GetAssetTypeGateway();

        // GET: AssetTypes
        public ActionResult Index()
        {
            return View(db.ReadAll());
        }

        // GET: AssetTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            AssetType assetType = db.Read(id.Value);
            if (assetType == null)
            {
                return HttpNotFound();
            }
            return View(assetType);
        }

        // GET: AssetTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AssetTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description")] AssetType assetType)
        {
            if (ModelState.IsValid)
            {
                db.Create(assetType);
                return RedirectToAction("Index");
            }

            return View(assetType);
        }

        // GET: AssetTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetType assetType = db.Read(id.Value);
            if (assetType == null)
            {
                return HttpNotFound();
            }
            return View(assetType);
        }

        // POST: AssetTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description")] AssetType assetType)
        {
            if (ModelState.IsValid)
            {
                db.Update(assetType);
                return RedirectToAction("Index");
            }
            return View(assetType);
        }

        // GET: AssetTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetType assetType = db.Read(id.Value);
            if (assetType == null)
            {
                return HttpNotFound();
            }
            return View(assetType);
        }

        // POST: AssetTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AssetType assetType = db.Read(id);
            db.Delete(assetType);
            return RedirectToAction("Index");
        }

       
    }
}
