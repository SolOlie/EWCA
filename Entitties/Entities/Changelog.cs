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
        [Display(Name = "Beskrivelse")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public int  AssetId { get; set; }
        public Asset Asset { get; set; }
        [Display(Name = "Minutter")]
        public int Minutes { get; set; }
        [Display(Name = "Dato")]
        [DataType(DataType.Date)]
        public DateTime ChangedDate { get; set; }

    }
}
