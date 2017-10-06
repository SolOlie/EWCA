using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DB;
using Entities.Entities;

namespace DAL.Repositories
{
    class ManufacturerRepo : IRepository<Manufacturer>
    {
        public Manufacturer Create(Manufacturer t)
        {
            using(var ctx = new CADBContext())
            {
                Manufacturer a = ctx.Manufacturers.Add(t);
                ctx.SaveChanges();
                return a;
            }
        }

        public Manufacturer Read(int id)
        {
            using (var ctx = new CADBContext())
            {
                return ctx.Manufacturers.FirstOrDefault(x => x.Id == id);
            }
        }

        public List<Manufacturer> ReadAll()
        {
            using (var ctx = new CADBContext())
            {
                var c = ctx.Manufacturers.Include(m => m.Assets).ToList();
                return c;
            }
        }

        public bool Delete(Manufacturer t)
        {
            using (var ctx = new CADBContext())
            {
                if (ManufacturerExists(t.Id, ctx))
                {
                    ctx.Manufacturers.Attach(t);
                    ctx.Manufacturers.Remove(t);
                    ctx.SaveChanges();
                    return true;
                }
                return false;


            }
        }
        private bool ManufacturerExists(int tId, CADBContext ctx)
        {
            if (ctx.AssetTypes.Count(a => a.Id == tId) > 0)
            {
                return true;
            }
            return false;
        }

        public bool Update(Manufacturer t)
        {
            using (var ctx = new CADBContext())
            {
                ctx.Entry(t).State = EntityState.Modified;
                try
                {
                    ctx.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManufacturerExists(t.Id, ctx))
                    {
                        return false;
                    }
                    else
                    {
                        throw;
                    }
                }
                return true;
            }
        }

        public List<Manufacturer> ReadAllWithFk(int id)
        {
            throw new NotImplementedException();
        }
    }
}
