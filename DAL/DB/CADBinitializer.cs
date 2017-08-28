using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Entities;

namespace DAL.DB
{
    class CADBinitializer : DropCreateDatabaseIfModelChanges<CADBContext>
    {
        protected override void Seed(CADBContext context)
        {
            
        }
    }
}
