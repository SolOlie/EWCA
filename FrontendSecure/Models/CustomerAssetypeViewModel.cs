using System.Collections.Generic;
using Entities.Entities;

namespace FrontendSecure.Models
{
    public class CustomerAssetypeViewModel
    {
        public Customer Customer { get; set; }
        public List<AssetType> AssetTypes { get; set; }
        public List<User> Users { get; set; }
        public List<Asset> SortedAssets { get; set; }
    }
}