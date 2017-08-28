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
using DAL.DB;
using Entities.Entities;

namespace EWCustomerAccountingBackend.Controllers
{
    public class Assets1Controller : ApiController
    {
        private CADBContext db = new CADBContext();

        // GET: api/Assets1
        public IQueryable<Asset> GetAssets()
        {
            return db.Assets;
        }

        // GET: api/Assets1/5
        [ResponseType(typeof(Asset))]
        public IHttpActionResult GetAsset(int id)
        {
            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                return NotFound();
            }

            return Ok(asset);
        }

        // PUT: api/Assets1/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAsset(int id, Asset asset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != asset.Id)
            {
                return BadRequest();
            }

            db.Entry(asset).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Assets1
        [ResponseType(typeof(Asset))]
        public IHttpActionResult PostAsset(Asset asset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Assets.Add(asset);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = asset.Id }, asset);
        }

        // DELETE: api/Assets1/5
        [ResponseType(typeof(Asset))]
        public IHttpActionResult DeleteAsset(int id)
        {
            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                return NotFound();
            }

            db.Assets.Remove(asset);
            db.SaveChanges();

            return Ok(asset);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AssetExists(int id)
        {
            return db.Assets.Count(e => e.Id == id) > 0;
        }
    }
}