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
using DAL.Repositories;
using Entities.Entities;

namespace EWCustomerAccountingBackend.Controllers
{
    //[Authorize]
    public class PortsController : ApiController
    {
        private IRepository<Port> dbp = new Facade().GetPortRepo();

        // GET: api/Portes
        public List<Port> GetPorts()
        {
            return dbp.ReadAll();
        }

        // GET: api/Portes/5
        [ResponseType(typeof(Port))]
        public IHttpActionResult GetPort(int id)
        {
            Port s = dbp.Read(id);
            if (s == null)
            {
                return NotFound();
            }

            return Ok(s);
        }

        // PUT: api/Portes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPort(int id, Port s)
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
                dbp.Update(s);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Portes
        [ResponseType(typeof(Port))]
        public IHttpActionResult PostPort(Port s)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dbp.Create(s);
            return CreatedAtRoute("DefaultApi", new { id = s.Id }, s);
        }

        // DELETE: api/Portes/5
        [ResponseType(typeof(Port))]
        public IHttpActionResult DeletePort(int id)
        {
            Port s = dbp.Read(id);
            if (s == null)
            {
                return NotFound();
            }

            dbp.Delete(s);
            return Ok(s);
        }
        public List<Port> GetPortsWithFk(int id)
        {
            return dbp.ReadAllWithFk(id);
        }
    }
}