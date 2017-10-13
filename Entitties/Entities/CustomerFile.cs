using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    [TrackChanges]
    public class CustomerFile
    {
        
            public int Id { get; set; }
            [SkipTracking]
            public int CustomerContentTypeId { get; set; }
            [SkipTracking]
            public CustomerContentType CustomerContentType { get; set; }
            [SkipTracking]
            public int CustomerId { get; set; }
            public Customer Customer { get; set; }
            [Display(Name = "Filnavn")]
            public string Name { get; set; }
            [SkipTracking]
            public string ContentType { get; set; }


        
    }
}
