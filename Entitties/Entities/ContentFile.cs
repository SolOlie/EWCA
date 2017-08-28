using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
   public class ContentFile
    {
       public int Id { get; set; }
       public byte[] Content { get; set; }
       public int FileId { get; set; }
       public File File { get; set; }
    }
}
