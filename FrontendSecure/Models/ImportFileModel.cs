using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Models
{
    public class ImportFileModel
    {
        public Customer Customer { get; set; }
        public List<Asset> Assets { get; set; }
        public List<AssetType> AssetTypes { get; set; }
    }
}