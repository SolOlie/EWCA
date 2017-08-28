using System.Collections.Generic;
using Entities.Entities;

namespace FrontendSecure.Models
{
    public class SearchModel
    {
        public List<User> Users { get; set; }
        public List<Asset> Assets { get; set; }
        public List<Customer> Customers { get; set; }

    }
}