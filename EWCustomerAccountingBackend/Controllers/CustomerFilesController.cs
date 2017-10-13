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
    public class CustomerFilesController : ApiController
    {
        private IRepository<CustomerFile> db = new Facade().GetCustomerFileRepo();
        

        // GET: api/CustomerFiles
        public List<CustomerFile> GetCustomerFiles()
        {
            return db.ReadAll();
        }

        // GET: api/CustomerFiles/5
        [ResponseType(typeof(CustomerFile))]
        public IHttpActionResult GetCustomerFile(int id)
        {
            CustomerFile asset = db.Read(id);
            if (asset == null)
            {
                return NotFound();
            }

            return Ok(asset);
        }

        // PUT: api/CustomerFiles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomerFile(int id, CustomerFile asset)
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

        // POST: api/CustomerFiles
        [ResponseType(typeof(CustomerFile))]
        public IHttpActionResult PostCustomerFile(CustomerFile file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Create(file);
            return CreatedAtRoute("DefaultApi", new { id = file.Id }, file);
        }

        // DELETE: api/CustomerFiles/5
        [ResponseType(typeof(CustomerFile))]
        public IHttpActionResult DeleteCustomerFile(int id)
        {
            CustomerFile file = db.Read(id);
            if (file == null)
            {
                return NotFound();
            }
            db.Delete(file);

            return Ok(file);
        }
        public List<CustomerFile> GetCustomerFilesWithFk(int id)
        {
            var a = db.ReadAllWithFk(id);

            return a;
        }
       
    }
}