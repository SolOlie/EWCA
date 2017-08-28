using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Gateways.SecureGateways
{
    public class AssetTypeGatewaySecure: IServiceGateway<AssetType>
    {
        public AssetType Create(AssetType t)
        {
            var AssetType = WebapiService.instance.PostAsync<AssetType>("/api/AssetTypes/PostAssetType", t, HttpContext.Current.User.Identity.Name).Result;
            return AssetType;
        }

        public AssetType Read(int id)
        {
            var AssetType = WebapiService.instance.GetAsync<AssetType>("/api/AssetTypes/GetAssetType/" + id, HttpContext.Current.User.Identity.Name).Result;
            return AssetType;
        }

        public List<AssetType> ReadAll()
        {
            var AssetTypes = WebapiService.instance.GetAsync<List<AssetType>>("/api/AssetTypes/GetAssetTypes", HttpContext.Current.User.Identity.Name).Result;
            return AssetTypes;
        }

        public bool Delete(AssetType t)
        {
            var AssetType = WebapiService.instance.DeleteAsync<AssetType>("/api/AssetTypes/DeleteAssetType/" + t.Id, HttpContext.Current.User.Identity.Name).Result;
            return AssetType;
        }

        public bool Update(AssetType t)
        {
            var AssetType = WebapiService.instance.PutAsync("/api/AssetTypes/PutAssetType/" + t.Id, t, HttpContext.Current.User.Identity.Name).Result;
            return AssetType;
        }

        public List<AssetType> ReadAllWithFk(int id)
        {
            throw new NotImplementedException();
        }
    }
}