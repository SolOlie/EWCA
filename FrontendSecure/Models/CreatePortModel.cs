using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Models
{
    public class CreatePortModel
    {
        public int  customerId { get; set; }
        public int uplinkid { get; set; }
        public int assetreturnid { get; set; }
        public Port Port { get; set; }
        public List<Asset> Assets { get; set; }
    }
}