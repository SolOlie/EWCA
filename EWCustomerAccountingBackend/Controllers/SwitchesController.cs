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
    public class SwitchesController : ApiController
    {
        private IRepository<Switch> db = new Facade().GetSwitchRepo();

        // GET: api/Switches
        public List<Switch> GetSwitches()
        {
            return db.ReadAll();
        }

        // GET: api/Switches/5
        [ResponseType(typeof(Switch))]
        public IHttpActionResult GetSwitch(int id)
        {
            Switch s = db.Read(id);
            if (s == null)
            {
                return NotFound();
            }

            return Ok(s);
        }

        // PUT: api/Switches/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSwitch(int id, Switch s)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != s.Id)
            {
                return BadRequest();
            }

           

            try
            {
                db.Update(s);
            }
            catch (DbUpdateConcurrencyException)
            {
               return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Switches
        [ResponseType(typeof(Switch))]
        public IHttpActionResult PostSwitch(Switch s)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Create(s);
            return CreatedAtRoute("DefaultApi", new { id = s.Id }, s);
        }

        // DELETE: api/Switches/5
        [ResponseType(typeof(Switch))]
        public IHttpActionResult DeleteSwitch(int id)
        {
            Switch s = db.Read(id);
            if (s == null)
            {
                return NotFound();
            }

            db.Delete(s);
            return Ok(s);
        }
        public List<Switch> GetSwitchesWithFk(int id)
        {
            return db.ReadAllWithFk(id);
        }
    }
}