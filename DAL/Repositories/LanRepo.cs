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
    class LanRepo : IRepository<Lan>
    {
        public Lan Create(Lan t)
        {
            using (var ctx = new CADBContext())
            {
                ctx.Entry(t.Customer).State = EntityState.Unchanged;
                Lan a = ctx.Lans.Add(t);

                ctx.SaveChanges();
                return a;
            }
        }

        public Lan Read(int id)
        {
            using (var ctx = new CADBContext())
            {
                return ctx.Lans.Include(m => m.Customer).FirstOrDefault(x => x.Id == id);
            }
        }

        public List<Lan> ReadAll()
        {
            using (var ctx = new CADBContext())
            {
                var c = ctx.Lans.Include(m => m.Customer).ToList();
                return c;
            }
        }

        public bool Delete(Lan t)
        {
            using (var ctx = new CADBContext())
            {
                if (LanExists(t.Id, ctx))
                {
                    ctx.Lans.Attach(t);
                    ctx.Lans.Remove(t);
                    ctx.SaveChanges();
                    return true;
                }
                return false;


            }
        }

        private bool LanExists(int tId, CADBContext ctx)
        {
            if (ctx.Lans.Count(a => a.Id == tId) > 0)
            {
                return true;
            }
            return false;

        }

        public bool Update(Lan t)
        {
            using (var ctx = new CADBContext())
            {
                ctx.Lans.Attach(t);
                ctx.Entry(t).State = EntityState.Modified;
                try
                {
                    ctx.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LanExists(t.Id, ctx))
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

        public List<Lan> ReadAllWithFk(int id)
        {
            using (var ctx = new CADBContext())
            {
                var c = ctx.Lans.Include(m => m.Customer).Where(x => x.Customer.Id == id).ToList();
                return c;
            }
        }
    }
}
