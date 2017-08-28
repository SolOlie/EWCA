using System;
using System.Collections.Generic;
using System.Net.Http;
using Entities.Entities;

namespace FrontendSecure.Gateways.UnsecureGateways
{
    class ChangelogGateway: AServiceGateway<Changelog>
    {
        protected override Changelog CreatePost(Changelog t, HttpClient client)
        {
            HttpResponseMessage response = client.PostAsJsonAsync("api/Changelogs/PostChangelog", t).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<Changelog>().Result;
            }
            return null;
        }

        protected override bool DeleteDel(Changelog t, HttpClient client)
        {
            var response = client.DeleteAsync("/api/Changelogs/DeleteChangelog/" + t.Id).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        protected override Changelog ReadOne(int id, HttpClient client)
        {
            HttpResponseMessage response = client.GetAsync("api/Changelogs/GetChangelog/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<Changelog>().Result;
            }
            return null;
        }

        protected override List<Changelog> ReadAllRead(HttpClient client)
        {
            HttpResponseMessage response = client.GetAsync("api/Changelogs/GetChangelogs/").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<Changelog>>().Result;
            }
            return new List<Changelog>();
        }

        protected override List<Changelog> ReadAllWithFkRead(int id, HttpClient client)
        {
            throw new NotImplementedException();
        }

        protected override bool UpdatePut(Changelog t, HttpClient client)
        {
            var response = client.PutAsJsonAsync("api/Changelogs/PutChangelog/" + t.Id, t).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
    }

