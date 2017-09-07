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
    [Authorize]
    public class ChangelogsController : ApiController
    {
        private IRepository<Changelog> db = new Facade().GetChangelogRepo();

        // GET: api/Changelogs
        public List<Changelog> GetChangelogs()
        {
            return db.ReadAll();
        }
        public List<Changelog> GetChangelogWithFk(int id)
        {
            return db.ReadAllWithFk(id);
        }

        // GET: api/Changelogs/5
        [ResponseType(typeof(Changelog))]
        public IHttpActionResult GetChangelog(int id)
        {
            Changelog changelog = db.Read(id);
            if (changelog == null)
            {
                return NotFound();
            }

            return Ok(changelog);
        }

        // PUT: api/Changelogs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutChangelog(int id, Changelog changelog)
        {
            foreach (var key in ModelState.Keys)
                if (key.Split('.').Length > 2)
                    ModelState[key].Errors.Clear();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != changelog.Id)
            {
                return BadRequest();
            }

            db.Update(changelog);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Changelogs
        [ResponseType(typeof(Changelog))]
        public IHttpActionResult PostChangelog(Changelog changelog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Create(changelog);

            return CreatedAtRoute("DefaultApi", new { id = changelog.Id }, changelog);
        }

        // DELETE: api/Changelogs/5
        [ResponseType(typeof(Changelog))]
        public IHttpActionResult DeleteChangelog(int id)
        {
            Changelog changelog = db.Read(id);
            if (changelog == null)
            {
                return NotFound();
            }

            db.Delete(changelog);

            return Ok(changelog);
        }

    }
}