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

                var asset = ctx.Assets.Include(x => x.Port).FirstOrDefault(x => x.Id == t.AssetId);
                if (asset?.Port != null)
                {
                    var p = asset.Port;
                    if (PortExists(p.Id, ctx))
                    {
                        ctx.Ports.Attach(p);
                        ctx.Ports.Remove(p);
                    }
                }

                t.Asset = asset;
                var pp = ctx.Ports.Add(t);
                if (asset != null)
                {

                    asset.PortId = pp.Id;
                    ctx.Assets.Attach(asset);
                    ctx.Entry(asset).State = EntityState.Modified;


                }

                // ctx.Entry(t.Asset).State = EntityState.Unchanged;
                ctx.SaveChanges();

                return t;
            }
        }

        public Port Read(int id)
        {
            using (var ctx = new CADBContext())
            {
                var a = ctx.Ports.Include(y => y.Asset).Include(x => x.Switch).FirstOrDefault(x => x.Id == id);
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
                var porttodelete = new Port(){Id = t.Id};
                Delete(porttodelete);
                var asset = ctx.Assets.Include(x => x.Port).FirstOrDefault(x => x.Id == t.AssetId);
                if (asset?.Port != null)
                {
                    var p = asset.Port;
                    if (PortExists(p.Id, ctx))
                    {
                        ctx.Ports.Attach(p);
                        ctx.Ports.Remove(p);
                    }
                }

                t.Asset = asset;
                var pp = ctx.Ports.Add(t);
                if (asset != null)
                {

                    asset.PortId = pp.Id;
                    ctx.Assets.Attach(asset);
                    ctx.Entry(asset).State = EntityState.Modified;


                }

                //var asset = ctx.Assets.AsNoTracking().Include(x => x.Port).FirstOrDefault(x => x.Id == t.AssetId);
                //if (asset?.Port != null && asset.Port.Id != t.Id)
                //{
                //    var p = asset.Port;
                //    if (PortExists(p.Id, ctx))
                //    {
                //        ctx.Ports.Attach(p);
                //        ctx.Ports.Remove(p);
                //    }

                  
                //}
                //if (asset != null && asset?.Port?.Id != t.Id)
                //{
                //    asset.PortId = t.Id;
                //    ctx.Assets.Attach(asset);
                //    ctx.Entry(asset).State = EntityState.Modified;
                   
                //}
                //     t.Asset = asset;
               

                //ctx.Ports.Attach(t);
                //ctx.Entry(t).State = EntityState.Modified;
                try
                {
                    ctx.SaveChanges();
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
                var afk = ctx.Ports.Include(y => y.Asset).Include(x => x.Switch).Where(c => c.SwitchId == id).ToList();
                return afk;
            }
        }
    }
}
