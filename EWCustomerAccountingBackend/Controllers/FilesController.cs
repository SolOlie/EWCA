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
    public class FilesController : ApiController
    {
        private IRepository<File> db = new Facade().GetFileRepo();

        // GET: api/Files
        public List<File> GetFiles()
        {
            return db.ReadAll();
        }

        // GET: api/Files/5
        [ResponseType(typeof(File))]
        public IHttpActionResult GetFile(int id)
        {
            File file = db.Read(id);
            if (file == null)
            {
                return NotFound();
            }

            return Ok(file);
        }

        // PUT: api/Files/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFile(int id, File file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != file.Id)
            {
                return BadRequest();
            }
            db.Update(file);
           return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Files
        [ResponseType(typeof(File))]
        public IHttpActionResult PostFile(File file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Create(file);
            return CreatedAtRoute("DefaultApi", new { id = file.Id }, file);
        }

        // DELETE: api/Files/5
        [ResponseType(typeof(File))]
        public IHttpActionResult DeleteFile(int id)
        {
            File file = db.Read(id);
            if (file == null)
            {
                return NotFound();
            }
            db.Delete(file);

            return Ok(file);
        }
    }
}