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
    class PortRepo : IRepository<Port>
    {
        public Port Create(Port t)
        {
            using (var ctx = new CADBContext())
            {
                //if (t.Switch != null)
                //{
                //    ctx.Ports.AddRange(t.Switch.Ports);
                //}
                
                //    ctx.Entry(t.Switch).State = EntityState.Unchanged;

             
                var asset = ctx.Assets.FirstOrDefault(x => x.Id == t.AssetId);
                t.Asset = asset;
                    ctx.Ports.Add(t);
                ctx.Entry(t.Asset).State = EntityState.Unchanged;
                ctx.SaveChanges();
                return t;
            }
        }

        public Port Read(int id)
        {
            using (var ctx = new CADBContext())
            {
                var a = ctx.Ports.Include(y => y.Asset).Include(x=>x.Switch).FirstOrDefault(x => x.Id == id);
                return a;
            }
        }

        public List<Port> ReadAll()
        {
            using (var ctx = new CADBContext())
            {
                var a = ctx.Ports.Include(y => y.Asset).Include(x => x.Switch).ToList();
                return a;
            }
        }

        public bool Delete(Port t)
        {
            using (var ctx = new CADBContext())
            {
                if (PortExists(t.Id, ctx))
                {
                    ctx.Ports.Attach(t);
                    ctx.Ports.Remove(t);
                    ctx.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        bool PortExists(int id, CADBContext ctx)
        {
            if (ctx.Ports.Count(a => a.Id == id) > 0)
            {
                return true;
            }
            return false;
        }

        public bool Update(Port t)
        {
            using (var ctx = new CADBContext())
            {
                ctx.Ports.Attach(t);
                try
                {
                    ctx.SaveChanges(t.Id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PortExists(t.Id, ctx))
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

        public List<Port> ReadAllWithFk(int id)
        {
            using (var ctx = new CADBContext())
            {
                var afk = ctx.Ports.Include(y => y.Asset).Include(x => x.Switch).Where(c=>c.SwitchId == id).ToList();
                return afk;
            }
        }
    }
}
