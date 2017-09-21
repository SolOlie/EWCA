using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Models
{
    public class FirewallListPartialModel
    {
        public int customerid { get; set; }
        public List<Firewall> Firewalls { get; set; }
    }
}