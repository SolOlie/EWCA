using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
   public class Manufacturer
    {
       public int Id { get; set; }
       public string manufacturer { get; set; }
       public List<Asset> Assets { get; set; }
    }
}
