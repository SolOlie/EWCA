using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Gateways.SecureGateways
{
    public class AssetGatewaySecure :IServiceGateway<Asset>
    {
        public Asset Create(Asset t)
        {
            var Asset = WebapiService.instance.PostAsync<Asset>("/api/Assets/PostAsset", t, HttpContext.Current.User.Identity.Name).Result;
            return Asset;
        }

        public Asset Read(int id)
        {
            var Asset = WebapiService.instance.GetAsync<Asset>("/api/Assets/GetAsset/" + id, HttpContext.Current.User.Identity.Name).Result;
            return Asset;
        }

        public List<Asset> ReadAll()
        {
            var Assets = WebapiService.instance.GetAsync<List<Asset>>("/api/Assets/GetAssets", HttpContext.Current.User.Identity.Name).Result;
            return Assets;
        }

        public bool Delete(Asset t)
        {
            var Asset = WebapiService.instance.DeleteAsync<Asset>("/api/Assets/DeleteAsset/" + t.Id, HttpContext.Current.User.Identity.Name).Result;
            return Asset;
        }

        public bool Update(Asset t)
        {
            var Asset = WebapiService.instance.PutAsync<Asset>("/api/Assets/PutAsset/" + t.Id, t, HttpContext.Current.User.Identity.Name).Result;
            return Asset;
        }

        public List<Asset> ReadAllWithFk(int id)
        {
            var Asset = WebapiService.instance.GetAsync<List<Asset>>("/api/Assets/GetAssetsWithFk/" + id, HttpContext.Current.User.Identity.Name).Result;
            return Asset;
        }
    }
}