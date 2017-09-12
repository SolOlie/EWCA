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
using FrontendSecure.Gateways;

namespace FrontendSecure.Controllers
{
    public class PortsController : ApiController
    {
        

        // GET: api/Ports
        public List<Port> GetPorts()
        {
            throw new NotImplementedException();
        }

        // GET: api/Ports/5
        [ResponseType(typeof(Port))]
        public IHttpActionResult GetPort(int id)
        {
            throw new NotImplementedException();
        }

        // PUT: api/Ports/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPort(int id, Port port)
        {
            throw new NotImplementedException();

        }

        // POST: api/Ports
        [ResponseType(typeof(Port))]
        public IHttpActionResult PostPort(Port port)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/Ports/5
        [ResponseType(typeof(Port))]
        public IHttpActionResult DeletePort(int id)
        {
            throw new NotImplementedException();
        }
        public List<Port> GetPortsWithFk(int id)
        {
            throw new NotImplementedException();
        }
    }
}