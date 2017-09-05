using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities.Entities;

namespace FrontendSecure.Models
{
    [TrackChanges]
    public class CreateUserWithCustomModel
    {
        public List<string> Users { get; set; }
        public int CustomerId { get; set; }
        public User User { get; set; }
    }
}