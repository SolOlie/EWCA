﻿using System;
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
    //[Authorize]
    public class AssetsController : ApiController
    {
        private IRepository<Asset> db = new Facade().GetAssetRepo();
        private IRepository<Switch> dbs = new Facade().GetSwitchRepo();
        
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
            var a = db.ReadAllWithFk(id);

            return a;
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
            var t = db.Create(asset);
            if (t.Type.Description.ToLower().Trim().Equals("switch"))
            {
               var s = dbs.Create(new Switch()
                {
                    Asset = t,
                    Customer = t.Customer,
                    Name = asset.Name,
                });

                if (s != null)
                {
                    var asse = db.Read(t.Id);
                    asse.SwitchId = s.Id;
                    db.Update(asse);
                }
            }


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