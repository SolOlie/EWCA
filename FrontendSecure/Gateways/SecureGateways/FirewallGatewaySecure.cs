using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Gateways.SecureGateways
{
    public class FirewallGatewaySecure : IServiceGateway<Firewall>
    {
        public Firewall Create(Firewall t)
        {
            var Firewall = WebapiService.instance.PostAsync<Firewall>("/api/Firewalls/PostFirewall", t, HttpContext.Current.User.Identity.Name).Result;
            return Firewall;
        }

        public Firewall Read(int id)
        {
            var Firewall = WebapiService.instance.GetAsync<Firewall>("/api/Firewalls/GetFirewall/" + id, HttpContext.Current.User.Identity.Name).Result;
            return Firewall;
        }

        public List<Firewall> ReadAll()
        {
            var Firewalls = WebapiService.instance.GetAsync<List<Firewall>>("/api/Firewalls/GetFirewalls", HttpContext.Current.User.Identity.Name).Result;
            return Firewalls;
        }

        public bool Delete(Firewall t)
        {
            var Firewall = WebapiService.instance.DeleteAsync<Firewall>("/api/Firewalls/DeleteFirewall/" + t.Id, HttpContext.Current.User.Identity.Name).Result;
            return Firewall;
        }

        public bool Update(Firewall t)
        {
            var Firewall = WebapiService.instance.PutAsync<Firewall>("/api/Firewalls/PutFirewall/" + t.Id, t, HttpContext.Current.User.Identity.Name).Result;
            return Firewall;
        }

        public List<Firewall> ReadAllWithFk(int id)
        {
            var Firewall = WebapiService.instance.GetAsync<List<Firewall>>("/api/Firewalls/GetFirewallsWithFk/" + id, HttpContext.Current.User.Identity.Name).Result;
            return Firewall;
        }
    }
}