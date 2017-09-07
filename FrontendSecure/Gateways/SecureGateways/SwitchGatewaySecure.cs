using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Gateways.SecureGateways
{
    public class SwitchGatewaySecure : IServiceGateway<Switch>
    {
        public Switch Create(Switch t)
        {
            var Switch = WebapiService.instance.PostAsync<Switch>("/api/Switches/PostSwitch", t, HttpContext.Current.User.Identity.Name).Result;
            return Switch;
        }

        public Switch Read(int id)
        {
            var Switch = WebapiService.instance.GetAsync<Switch>("/api/Switches/GetSwitch/" + id, HttpContext.Current.User.Identity.Name).Result;
            return Switch;
        }

        public List<Switch> ReadAll()
        {
            var Switchs = WebapiService.instance.GetAsync<List<Switch>>("/api/Switches/GetSwitches", HttpContext.Current.User.Identity.Name).Result;
            return Switchs;
        }

        public bool Delete(Switch t)
        {
            var Switch = WebapiService.instance.DeleteAsync<Switch>("/api/Switches/DeleteSwitch/" + t.Id, HttpContext.Current.User.Identity.Name).Result;
            return Switch;
        }

        public bool Update(Switch t)
        {
            var Switch = WebapiService.instance.PutAsync<Switch>("/api/Switches/PutSwitch/" + t.Id, t, HttpContext.Current.User.Identity.Name).Result;
            return Switch;
        }

        public List<Switch> ReadAllWithFk(int id)
        {
            var Switch = WebapiService.instance.GetAsync<List<Switch>>("/api/Switches/GetSwitchesWithFk/" + id, HttpContext.Current.User.Identity.Name).Result;
            return Switch;
        }
    }
}