using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using DAL;
using DAL.DB;
using DAL.Repositories;
using Entities.Entities;
using EWCustomerAccountingBackend.Models;

namespace EWCustomerAccountingBackend.Controllers
{
    
    public class AssetsController : ApiController
    {
        private IRepository<Asset> db = new Facade().GetAssetRepo();
        
        // GET: api/Assets
        public List<Asset> GetAssets()
        {
            return db.ReadAll();
        }

        /// <summary>
        /// Get all assets from a Customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Asset> GetAssetsWithFk(int id)
        {
            return db.ReadAllWithFk(id);
        }


        // GET: api/Assets/5
        [ResponseType(typeof(Asset))]
        public IHttpActionResult GetAsset(int id)
        {
            Asset asset = db.Read(id);
            if (asset == null)
            {
                return NotFound();
            }

            return Ok(asset);
        }

        // PUT: api/Assets/5
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
            db.Update(asset);
          

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Assets
        [ResponseType(typeof(Asset))]
        public IHttpActionResult PostAsset(Asset asset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Create(asset);


            return CreatedAtRoute("DefaultApi", new { id = asset.Id }, asset);
        }
       

        // DELETE: api/Assets/5
        [ResponseType(typeof(Asset))]
        public IHttpActionResult DeleteAsset(int id)
        {
            Asset asset = db.Read(id);
            if (asset == null)
            {
                return NotFound();
            }

            db.Delete(asset);

            return Ok(asset);
        }

    }
}