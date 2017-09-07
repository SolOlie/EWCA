using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Models
{
    public class CreateSwitchModel
    {
        public List<Asset> Assets { get; set; }
        public int CustomerId { get; set; }
        public Switch Switch { get; set; }
    }
}