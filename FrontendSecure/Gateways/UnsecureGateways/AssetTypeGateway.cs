using System;
using System.Collections.Generic;
using System.Net.Http;
using Entities.Entities;

namespace FrontendSecure.Gateways.UnsecureGateways
{
    class AssetTypeGateway : AServiceGateway<AssetType>
    {
        protected override AssetType CreatePost(AssetType t, HttpClient client)
        {
            HttpResponseMessage response = client.PostAsJsonAsync("api/AssetTypes/PostAssetType", t).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<AssetType>().Result;
            }
            return null;
        }

        protected override bool DeleteDel(AssetType t, HttpClient client)
        {
            var response = client.DeleteAsync("/api/AssetTypes/DeleteAssetType" + t.Id).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        protected override AssetType ReadOne(int id, HttpClient client)
        {
            HttpResponseMessage response = client.GetAsync("api/AssetTypes/GetAssetType" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<AssetType>().Result;
            }
            return null;
        }

        protected override List<AssetType> ReadAllRead(HttpClient client)
        {
            HttpResponseMessage response = client.GetAsync("api/AssetTypes/GetAssetTypes").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<AssetType>>().Result;
            }
            return new List<AssetType>();
        }


        protected override List<AssetType> ReadAllWithFkRead(int id, HttpClient client)
        {
            throw new NotImplementedException();
        }

        protected override bool UpdatePut(AssetType t, HttpClient client)
        {
            var response = client.PutAsJsonAsync("api/AssetTypes/PutAssetType/" + t.Id, t).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
