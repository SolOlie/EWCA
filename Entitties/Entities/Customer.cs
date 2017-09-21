using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Entities
{
    [TrackChanges]
    public class Customer 
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Firma")]
        public string Firm { get; set; }
        [Display(Name = "Adresse")]
        public string Address { get; set; }
        public List<User> ContactPersons{ get; set; }
        [Display(Name = "Dato")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }//Create date for customer
        [SkipTracking]
        public List<Asset> Assets { get; set; }

        public List<Switch> Switches { get; set; }

        public List<Lan> Lans { get; set; }
        public List<Firewall> Firewalls { get; set; }


    }
}
