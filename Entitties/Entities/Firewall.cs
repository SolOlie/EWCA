using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    public class Firewall
    {
        public int Id { get; set; }

        [Display(Name = "Services")]
        public string Protocol { get; set; }

        [Display(Name = "Source")]
        public string AllowedIps { get; set; }

        [Display(Name = "Udgående interface")]
        public string  InterfaceO { get; set; }
        [Display(Name = "Indgående interface")]
        public string  Interface { get; set; }

        public string Destination { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
