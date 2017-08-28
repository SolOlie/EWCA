using System.Collections.Generic;
using System.Net.Http;
using Entities.Entities;

namespace FrontendSecure.Gateways.UnsecureGateways
{
    class FileGateway : AServiceGateway<File>
    {
        protected override File CreatePost(File t, HttpClient client)
        {
            HttpResponseMessage response = client.PostAsJsonAsync("api/Files/PostFile", t).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<File>().Result;
            }
            return null;
        }

        protected override bool DeleteDel(File t, HttpClient client)
        {
            var response = client.DeleteAsync("/api/Files/DeleteFile/" + t.Id).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        protected override File ReadOne(int id, HttpClient client)
        {
            HttpResponseMessage response = client.GetAsync("api/Files/GetFile/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<File>().Result;
            }
            return null;
        }

        protected override List<File> ReadAllRead(HttpClient client)
        {
            HttpResponseMessage response = client.GetAsync("api/Files/GetFiles").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<File>>().Result;
            }
            return new List<File>();
        }

        protected override List<File> ReadAllWithFkRead(int id, HttpClient client)//id is asset id
        {
            HttpResponseMessage response = client.GetAsync("api/Files/GetFilesWithFk/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<File>>().Result;
            }
            return null;
        }


        protected override bool UpdatePut(File t, HttpClient client)
        {
            var response = client.PutAsJsonAsync("api/Files/PutFile/" + t.Id, t).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
