using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    public class Port
    {
        public int Id { get; set; }
        public int SwitchId { get; set; }
        public Switch Switch { get; set; }
        public int PortNumber { get; set; }
        public int AssetId { get; set; }
        [DisplayName("Asset")]
        public Asset Asset { get; set; }
        public string Trunk { get; set; }
        public string VLAN { get; set; }
        public string Note { get; set; }

    }
}
