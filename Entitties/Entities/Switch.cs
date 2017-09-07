using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    public class Switch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Port> Ports { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
