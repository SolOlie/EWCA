using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Models
{
    [TrackChanges]
    public class ISoftDeleteTableModel : ISoftDelete
    {
        public long Id { get; set; }

        public bool IsDeleted { get; set; }

        public string Description { get; set; }
        
        public bool SoftDelete { get; set; }

        public void Delete()
        {
            SoftDelete = true;
        }
    }
}