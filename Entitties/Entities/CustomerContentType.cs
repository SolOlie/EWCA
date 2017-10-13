using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    public class CustomerContentType
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public int FileId { get; set; }
        public CustomerFile CustomerFile { get; set; }
    }
}
