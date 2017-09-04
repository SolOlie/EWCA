using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities.Entities;

namespace FrontendSecure.Models
{
    [TrackChanges]
    public class CreateChangelogModel
    {
        public int AssetId { get; set; }
        public Asset Asset { get; set; }
        public List<User> Users { get; set; }
        public Changelog Changelog { get; set; }

    }
}