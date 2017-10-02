using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Entities.Entities
{
    public class Lan
    {
        public int Id { get; set; }
        [Display(Name = "Navn")]
        public string Name { get; set; }
        [Display(Name = "Netværk")]
        public string Network { get; set; }
        [Display(Name = "DHCP Server")]
        public string DhcpServer { get; set; }

        [Display(Name = "DNS")]
        public string Dns { get; set; }

        public string VLan { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
