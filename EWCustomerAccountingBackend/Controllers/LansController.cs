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
    public class LansController : ApiController
    {
        private IRepository<Lan> db = new Facade().GetLanRepo();

        // GET: api/Lans
        public List<Lan> GetLans()
        {
            return db.ReadAll();
        }

        public List<Lan> GetLansWithFk(int id)
        {
            return db.ReadAllWithFk(id);
        }

        // GET: api/Lans/5
        [ResponseType(typeof(Lan))]
        public IHttpActionResult GetLan(int id)
        {
            Lan lan = db.Read(id);
            if (lan == null)
            {
                return NotFound();
            }

            return Ok(lan);
        }

        // PUT: api/Lans/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLan(int id, Lan lan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lan.Id)
            {
                return BadRequest();
            }

            db.Update(lan);
          
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Lans
        [ResponseType(typeof(Lan))]
        public IHttpActionResult PostLan(Lan lan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           
            db.Create(lan);

            return CreatedAtRoute("DefaultApi", new { id = lan.Id }, lan);
        }

        // DELETE: api/Lans/5
        [ResponseType(typeof(Lan))]
        public IHttpActionResult DeleteLan(int id)
        {
            Lan lan = db.Read(id);
            if (lan == null)
            {
                return NotFound();
            }

            db.Delete(lan);
            

            return Ok(lan);
        }

     
    }
}