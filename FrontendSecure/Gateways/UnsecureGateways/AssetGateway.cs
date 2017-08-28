using System.Collections.Generic;
using System.Net.Http;
using Entities.Entities;

namespace FrontendSecure.Gateways.UnsecureGateways
{
    class AssetGateway: AServiceGateway<Asset>
    {
        protected override Asset CreatePost(Asset t, HttpClient client)
        {
            HttpResponseMessage response = client.PostAsJsonAsync("api/Assets/PostAsset", t).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<Asset>().Result;
            }
            return null;
        }

        protected override bool DeleteDel(Asset t, HttpClient client)
        {
            var response = client.DeleteAsync("/api/Assets/DeleteAsset/" + t.Id).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        protected override Asset ReadOne(int id, HttpClient client)
        {
            HttpResponseMessage response = client.GetAsync("api/Assets/GetAsset/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<Asset>().Result;
            }
            return null;
        }

        protected override List<Asset> ReadAllRead(HttpClient client)
        {
            HttpResponseMessage response = client.GetAsync("api/Assets/GetAssets").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<Asset>>().Result;
            }
            return new List<Asset>();
        }

        protected override List<Asset> ReadAllWithFkRead(int id, HttpClient client)//id is asset id
        {
            HttpResponseMessage response = client.GetAsync("api/Assets/GetAssetsWithFk/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<Asset>>().Result;
            }
            return null;
        }


        protected override bool UpdatePut(Asset t, HttpClient client)
        {
            var response = client.PutAsJsonAsync("api/Assets/PutAsset/" + t.Id, t).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
