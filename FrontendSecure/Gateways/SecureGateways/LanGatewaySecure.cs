using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Gateways.SecureGateways
{
    public class LanGatewaySecure : IServiceGateway<Lan>
    {
        public Lan Create(Lan t)
        {
            var Lan = WebapiService.instance.PostAsync<Lan>("/api/Lans/PostLan", t, HttpContext.Current.User.Identity.Name).Result;
            return Lan;
        }

        public Lan Read(int id)
        {
            var Lan = WebapiService.instance.GetAsync<Lan>("/api/Lans/GetLan/" + id, HttpContext.Current.User.Identity.Name).Result;
            return Lan;
        }

        public List<Lan> ReadAll()
        {
            var Lans = WebapiService.instance.GetAsync<List<Lan>>("/api/Lans/GetLans", HttpContext.Current.User.Identity.Name).Result;
            return Lans;
        }

        public bool Delete(Lan t)
        {
            var Lan = WebapiService.instance.DeleteAsync<Lan>("/api/Lans/DeleteLan/" + t.Id, HttpContext.Current.User.Identity.Name).Result;
            return Lan;
        }

        public bool Update(Lan t)
        {
            var Lan = WebapiService.instance.PutAsync<Lan>("/api/Lans/PutLan/" + t.Id, t, HttpContext.Current.User.Identity.Name).Result;
            return Lan;
        }

        public List<Lan> ReadAllWithFk(int id)
        {
            var Lan = WebapiService.instance.GetAsync<List<Lan>>("/api/Lans/GetLansWithFk/" + id, HttpContext.Current.User.Identity.Name).Result;
            return Lan;
        }
    }
}