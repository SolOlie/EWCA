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
       [Display(Name = "Navn")]
        public string Name { get; set; }
        
        public int? CustomerId { get; set; }
       [Display(Name = "Kunde")]
        public Customer Customer { get; set; }
        [Display(Name = "Beskrivelse")]
        public string Description { get; set; }

        public string Address { get; set; }
        [Display(Name = "Bruger")]
        public string Usedby { get; set; }// Email or name
        [Display(Name = "Lokation")]
        public string Location { get; set; }// building and room
        [Display(Name = "Oprettelses dato")]
        [DataType(DataType.Date)]
        public DateTime InstallationDate { get; set; }

        public List<Changelog> Changelogs { get; set; }

        public List<File> FileAttachments{ get; set; }

        public string Login { get; set; }//login credentials for the asset
        
        [SkipTracking]
        public string Password { get; set; }//login credentials for the asset
        [Display(Name = "IP Adresse")]
        public string IpAddress { get; set; }

        public string OS { get; set; }//operating system of the asset
        [Display(Name = "Notat")]
        public string Note { get; set; }

        public bool SoftDelete { get; set; }

        public string RAM { get; set; }

        public string HDD { get; set; }

        public Port Port { get; set; }
        public int PortId { get; set; }
        public Switch Switch { get; set; }
        public int SwitchId { get; set; }

    }
}
