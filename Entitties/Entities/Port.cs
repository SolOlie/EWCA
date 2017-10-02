using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    public class Port
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SwitchId { get; set; }
        public Switch Switch { get; set; }
        [Display(Name = "Port nr.")]
        public int PortNumber { get; set; }
        [DisplayName("Udstyr")]
        public string Asset { get; set; }
        public string Trunk { get; set; }
        public string VLAN { get; set; }
        public string Note { get; set; }

    }
}
