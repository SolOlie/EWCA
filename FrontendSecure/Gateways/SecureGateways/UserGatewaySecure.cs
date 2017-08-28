using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Gateways.SecureGateways
{
    public class UserGatewaySecure : IServiceGateway<User>
    {
        public User Create(User t)
        {
            var User = WebapiService.instance.PostAsync<User>("/api/Users/PostUser", t, HttpContext.Current.User.Identity.Name).Result;
            return User;
        }

        public User Read(int id)
        {
            
            var User = WebapiService.instance.GetAsync<User>("/api/Users/GetUser/" + id, HttpContext.Current.User.Identity.Name).Result;
            return User;
        }

        public List<User> ReadAll()
        {
            var Users = WebapiService.instance.GetAsync<List<User>>("/api/Users/GetUsers", HttpContext.Current.User.Identity.Name).Result;
            return Users;
        }

        public bool Delete(User t)
        {
            var User = WebapiService.instance.DeleteAsync<User>("/api/Users/DeleteUser/" + t.Id, HttpContext.Current.User.Identity.Name).Result;
            return User;
        }

        public bool Update(User t)
        {
            var User = WebapiService.instance.PutAsync("/api/Users/PutUser/" + t.Id, t, HttpContext.Current.User.Identity.Name).Result;
            return User;
        }

        public List<User> ReadAllWithFk(int id)
        {
            var Users = WebapiService.instance.GetAsync<List<User>>("/api/Users/GetUsersWithFk/"+id, HttpContext.Current.User.Identity.Name).Result;
            return Users;
        }
    }
}