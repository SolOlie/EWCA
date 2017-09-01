using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Models
{
    public class UsersListPartialModel
    {
        public List<User> Users { get; set; }
        public int CustomerId { get; set; }
    }
}