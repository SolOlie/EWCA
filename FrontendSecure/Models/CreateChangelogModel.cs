using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities.Entities;

namespace FrontendSecure.Models
{
    [TrackChanges]
    public class CreateChangelogModel
    {
        public Asset Asset { get; set; }
        public List<User> Users { get; set; }

    }
}