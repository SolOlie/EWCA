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
        public string Firm { get; set; }
        public string Address { get; set; }
        public List<User> ContactPersons{ get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }//Create date for customer
        [SkipTracking]
        public List<Asset> Assets { get; set; }


    }
}
