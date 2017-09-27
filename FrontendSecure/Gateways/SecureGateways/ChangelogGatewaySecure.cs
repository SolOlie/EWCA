using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Gateways.SecureGateways
{
    public class ChangelogGatewaySecure: IServiceGateway<Changelog>
    {
        public Changelog Create(Changelog t)
        {
            var Changelog = WebapiService.instance.PostAsync<Changelog>("/api/Changelogs/PostChangelog", t, HttpContext.Current.User.Identity.Name).Result;
            return Changelog;
        }

        public Changelog Read(int id)
        {
            var Changelog = WebapiService.instance.GetAsync<Changelog>("/api/Changelogs/GetChangelog/" + id, HttpContext.Current.User.Identity.Name).Result;
            return Changelog;
        }

        public List<Changelog> ReadAll()
        {
            var Changelogs = WebapiService.instance.GetAsync<List<Changelog>>("/api/Changelogs/GetChangelogs", HttpContext.Current.User.Identity.Name).Result;
            return Changelogs;
        }

        public bool Delete(Changelog t)
        {
            var Changelog = WebapiService.instance.DeleteAsync<Changelog>("/api/Changelogs/DeleteChangelog/" + t.Id, HttpContext.Current.User.Identity.Name).Result;
            return Changelog;
        }

        public bool Update(Changelog t)
        {
            var Changelog = WebapiService.instance.PutAsync("/api/Changelogs/PutChangelog/" + t.Id, t, HttpContext.Current.User.Identity.Name).Result;
            return Changelog;
        }

        public List<Changelog> ReadAllWithFk(int id)
        {
            var Changelog = WebapiService.instance.GetAsync<List<Changelog>>("/api/Changelogs/GetChangelogWithFk/" + id, HttpContext.Current.User.Identity.Name).Result;
            return Changelog;
        }
    }
}