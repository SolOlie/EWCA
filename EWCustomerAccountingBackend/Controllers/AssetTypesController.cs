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
using DAL;
using DAL.DB;
using DAL.Repositories;
using Entities.Entities;

namespace EWCustomerAccountingBackend.Controllers
{
    //[Authorize]
    public class AssetTypesController : ApiController
    {
        private IRepository<AssetType> db = new Facade().GetAssetTypeRepo();

        // GET: api/AssetTypes
        public List<AssetType> GetAssetTypes()
        {
            return db.ReadAll();
        }

        // GET: api/AssetTypes/5
        [ResponseType(typeof(AssetType))]
        public IHttpActionResult GetAssetType(int id)
        {
            AssetType assetType = db.Read(id);
            if (assetType == null)
            {
                return NotFound();
            }

            return Ok(assetType);
        }

        // PUT: api/AssetTypes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAssetType(int id, AssetType assetType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != assetType.Id)
            {
                return BadRequest();
            }

            db.Update(assetType);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AssetTypes
        [ResponseType(typeof(AssetType))]
        public IHttpActionResult PostAssetType(AssetType assetType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Create(assetType);

            return CreatedAtRoute("DefaultApi", new { id = assetType.Id }, assetType);
        }

        // DELETE: api/AssetTypes/5
        [ResponseType(typeof(AssetType))]
        public IHttpActionResult DeleteAssetType(int id)
        {
            AssetType assetType = db.Read(id);
            if (assetType == null)
            {
                return NotFound();
            }

            db.Delete(assetType);

            return Ok(assetType);
        }
    }
}