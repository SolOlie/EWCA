using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    public class Switch
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Port> Ports { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Asset Asset { get; set; }
        public int AssetId { get; set; }
    }
}
