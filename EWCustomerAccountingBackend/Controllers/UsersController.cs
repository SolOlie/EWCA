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
using EWCustomerAccountingBackend.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EWCustomerAccountingBackend.Controllers
{
    public class UsersController : ApiController
    {
        private IRepository<User> db = new Facade().GetUserRepo();

        // GET: api/Users
        public List<User> GetUsers()
        {
            return db.ReadAll();
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Read(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        public List<User> GetUsersWithFk(int id)
        {
            return db.ReadAllWithFk(id);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            db.Update(user);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           var u = db.Create(user);
            if (u == null)
            {
                return BadRequest("Email already used");
            }
            return CreatedAtRoute("DefaultApi", new { id = u.Id }, u);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Read(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Delete(user);

            return Ok(user);
        }
    }
}