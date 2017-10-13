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
    public class CustomerFilesController : ApiController
    {
        private CADBContext db = new CADBContext();

        // GET: api/CustomerFiles
        public IQueryable<CustomerFile> GetCustomerFiles()
        {
            return db.CustomerFiles;
        }

        // GET: api/CustomerFiles/5
        [ResponseType(typeof(CustomerFile))]
        public IHttpActionResult GetCustomerFile(int id)
        {
            CustomerFile customerFile = db.CustomerFiles.Find(id);
            if (customerFile == null)
            {
                return NotFound();
            }

            return Ok(customerFile);
        }

        // PUT: api/CustomerFiles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomerFile(int id, CustomerFile customerFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerFile.Id)
            {
                return BadRequest();
            }

            db.Entry(customerFile).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerFileExists(id))
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

        // POST: api/CustomerFiles
        [ResponseType(typeof(CustomerFile))]
        public IHttpActionResult PostCustomerFile(CustomerFile customerFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CustomerFiles.Add(customerFile);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = customerFile.Id }, customerFile);
        }

        // DELETE: api/CustomerFiles/5
        [ResponseType(typeof(CustomerFile))]
        public IHttpActionResult DeleteCustomerFile(int id)
        {
            CustomerFile customerFile = db.CustomerFiles.Find(id);
            if (customerFile == null)
            {
                return NotFound();
            }

            db.CustomerFiles.Remove(customerFile);
            db.SaveChanges();

            return Ok(customerFile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerFileExists(int id)
        {
            return db.CustomerFiles.Count(e => e.Id == id) > 0;
        }
    }
}