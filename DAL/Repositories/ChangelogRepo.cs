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
    class ChangelogRepo: IRepository<Changelog>
    {
        public Changelog Create(Changelog t)
            {
            using (var ctx = new CADBContext())
            {
                t.AssetId = t.Asset.Id;
                Changelog a = ctx.Changelogs.Add(t);
                ctx.Entry(t.User).State = EntityState.Unchanged;
                ctx.Entry(t.Asset).State = EntityState.Unchanged;
                ctx.SaveChanges();
                return a;
            }
        }

        public Changelog Read(int id)
        {
            using (var ctx = new CADBContext())
            {
                return ctx.Changelogs.Include(y => y.User).Include(x => x.Asset).Include(x=> x.Asset.Customer).FirstOrDefault(x => x.Id == id);
            }
        }

        public List<Changelog> ReadAll()
        {
            using (var ctx = new CADBContext())
            {
                return ctx.Changelogs.Include(y => y.User).Include(x => x.Asset).ToList();
            }
        }

        public bool Delete(Changelog t)
        {
            using (var ctx = new CADBContext())
            {
                if (ChangelogExists(t.Id, ctx))
                {
                    ctx.Changelogs.Attach(t);
                    ctx.Changelogs.Remove(t);
                    ctx.SaveChanges();
                    return true;
                }
                return false;


            }
        }

        private bool ChangelogExists(int tId, CADBContext ctx)
        {
            if (ctx.Changelogs.Count(a => a.Id == tId) > 0)
            {
                return true;
            }
            return false;
        }

        public bool Update(Changelog t)
        {
            using (var ctx = new CADBContext())
            {
                ctx.Entry(t.User).State = EntityState.Unchanged;
                ctx.Entry(t.Asset).State = EntityState.Unchanged;
                ctx.Entry(t).State = EntityState.Modified;
                try
                {
                    ctx.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChangelogExists(t.Id, ctx))
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

        public List<Changelog> ReadAllWithFk(int id)
        {
            using (var ctx = new CADBContext())
            {
                return ctx.Changelogs.Include(y => y.User).Include(x=>x.Asset).Where(x => x.Asset.Id == id).ToList();
            }
        }
    }
}
