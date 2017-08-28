using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Entities
{
    [TrackChanges]
    public class Changelog 
    {
        [Key]
        public int Id { get; set; }
        [SkipTracking]
        public int UserId { get; set; }
        public User User { get; set; }
        public string Description { get; set; }
        public Asset Asset { get; set; }
        public double Hours { get; set; }
        [DataType(DataType.Date)]
        public DateTime ChangedDate { get; set; }

    }
}
