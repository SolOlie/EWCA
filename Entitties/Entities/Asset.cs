using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entities
{
    [TrackChanges]
    public class Asset : ISoftDelete
    {
        [Key]
        public int Id { get; set; }
       
        public int? TypeId { get; set; }
        public AssetType Type { get; set; }
       
        public string Name { get; set; }
        
        public int? CustomerId { get; set; }
       
        public Customer Customer { get; set; }
        
        public string Description { get; set; }

        public string Address { get; set; }

        public string Usedby { get; set; }// Email or name

        public string Location { get; set; }// building and room

        [DataType(DataType.Date)]
        public DateTime InstallationDate { get; set; }

        public List<Changelog> Changelogs { get; set; }

        public List<File> FileAttachments{ get; set; }

        public string Login { get; set; }//login credentials for the asset
        [SkipTracking]
        public string Password { get; set; }//login credentials for the asset

        public string IpAddress { get; set; }

        public string OS { get; set; }//operating system of the asset

        public string Note { get; set; }

        public bool SoftDelete { get; set; }

    }
}
