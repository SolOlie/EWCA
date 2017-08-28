using System;
using System.Collections.Generic;
using System.Net.Http;
using Entities.Entities;

namespace FrontendSecure.Gateways.UnsecureGateways
{
    class UserGateway: AServiceGateway<User>
    {
        protected override User CreatePost(User t, HttpClient client)
        {
            HttpResponseMessage response = client.PostAsJsonAsync("api/Users/PostUser", t).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<User>().Result;
            }
            return null;
        }

        protected override bool DeleteDel(User t, HttpClient client)
        {
            var response = client.DeleteAsync("/api/Users/DeleteUser/" + t.Id).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        protected override User ReadOne(int id, HttpClient client)
        {
            HttpResponseMessage response = client.GetAsync("api/Users/GetUser/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<User>().Result;
            }
            return null;
        }

        protected override List<User> ReadAllRead(HttpClient client)
        {
            HttpResponseMessage response = client.GetAsync("api/Users/GetUsers").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<User>>().Result;
            }
            return new List<User>();
        }

        protected override List<User> ReadAllWithFkRead(int id, HttpClient client)
        {
            throw new NotImplementedException();
        }

        protected override bool UpdatePut(User t, HttpClient client)
        {
            var response = client.PutAsJsonAsync("api/Users/PutUser/" + t.Id, t).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
