using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Entities
{
    [TrackChanges]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        [SkipTracking]
        public string Password { get; set; }
        public List<Changelog> Changelogs { get; set; }
        [SkipTracking]
        public Customer IsContactForCustomer { get; set; }

    }
}
