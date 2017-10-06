using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities.Entities;

namespace FrontendSecure.Models
{
    [TrackChanges]
    public class CreateAssetModel : ISoftDelete
    {
        public List<Customer> Customers { get; set; }
        public Asset Asset { get; set; }
        public List<User> Users { get; set; }
        public List<AssetType> AssetTypes { get; set; }
        public int customerId { get; set; }
        public List<Asset> customerassetlist { get; set; }
        public bool SoftDelete { get; set; }
        public List<Manufacturer> Manufacturers { get; set; }
    }
}