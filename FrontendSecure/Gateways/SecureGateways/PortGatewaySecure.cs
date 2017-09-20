using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Gateways.SecureGateways
{
    public class PortGatewaySecure : IServiceGateway<Port>
    {
        public Port Create(Port t)
        {
            var Port = WebapiService.instance.PostAsync<Port>("/api/Ports/PostPort", t, HttpContext.Current.User.Identity.Name).Result;
            return Port;
        }

        public Port Read(int id)
        {
            var Port = WebapiService.instance.GetAsync<Port>("/api/Ports/GetPort/" + id, HttpContext.Current.User.Identity.Name).Result;
            return Port;
        }

        public List<Port> ReadAll()
        {
            var Ports = WebapiService.instance.GetAsync<List<Port>>("/api/Ports/GetPorts", HttpContext.Current.User.Identity.Name).Result;
            return Ports;
        }

        public bool Delete(Port t)
        {
            var Port = WebapiService.instance.DeleteAsync<Port>("/api/Ports/DeletePort/" + t.Id, HttpContext.Current.User.Identity.Name).Result;
            return Port;
        }

        public bool Update(Port t)
        {
            var Port = WebapiService.instance.PutAsync<Port>("/api/Ports/PutPort/" + t.Id, t, HttpContext.Current.User.Identity.Name).Result;
            return Port;
        }

        public List<Port> ReadAllWithFk(int id)
        {
            var Port = WebapiService.instance.GetAsync<List<Port>>("/api/Ports/GetPortsWithFk/" + id, HttpContext.Current.User.Identity.Name).Result;
            return Port;
        }
    }
}