using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Entities
{
    public class AssetType
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public List<Asset> Assets { get; set; }
    }
}
