using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    [TrackChanges]
    public class File
    {
        public int Id { get; set; }
        [SkipTracking]
        public int ContentFileId { get; set; }
        [SkipTracking]
        public ContentFile ContentFile { get; set; }
        [SkipTracking]
        public int AssetId { get; set; }
        public Asset Asset { get; set; }
        public string Name { get; set; }
        [SkipTracking]
        public string ContentType { get; set; }


    }
}
