using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities.Entities;

namespace FrontendSecure.Models
{
    [TrackChanges]
    public class CreateCustomerModel
    {
        public List<User> Users { get; set; }
        public Customer Customer { get; set; }
       
    }
}