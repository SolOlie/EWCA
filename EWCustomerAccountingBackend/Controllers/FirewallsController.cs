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
    public class FirewallsController : ApiController
    {
        private IRepository<Firewall> db = new Facade().GetFirewallRepo();

        // GET: api/Firewalls
        public List<Firewall> GetFirewalls()
        {
            return db.ReadAll();
        }

        public List<Firewall> GetFirewallsWithFk(int id)
        {
            return db.ReadAllWithFk(id);
        }

        // GET: api/Firewalls/5
        [ResponseType(typeof(Firewall))]
        public IHttpActionResult GetFirewall(int id)
        {
            Firewall firewall = db.Read(id);
            if (firewall == null)
            {
                return NotFound();
            }

            return Ok(firewall);
        }

        // PUT: api/Firewalls/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFirewall(int id, Firewall firewall)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != firewall.Id)
            {
                return BadRequest();
            }

            db.Update(firewall);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Firewalls
        [ResponseType(typeof(Firewall))]
        public IHttpActionResult PostFirewall(Firewall firewall)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Create(firewall);

            return CreatedAtRoute("DefaultApi", new { id = firewall.Id }, firewall);
        }

        // DELETE: api/Firewalls/5
        [ResponseType(typeof(Firewall))]
        public IHttpActionResult DeleteFirewall(int id)
        {
            Firewall firewall = db.Read(id);
            if (firewall == null)
            {
                return NotFound();
            }

            db.Delete(firewall);

            return Ok(firewall);
        }

    }
}