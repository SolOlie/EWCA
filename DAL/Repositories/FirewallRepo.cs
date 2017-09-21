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
    class FirewallRepo : IRepository<Firewall>
    {
        public Firewall Create(Firewall t)
        {
            using (var ctx = new CADBContext())
            {
                ctx.Entry(t.Customer).State = EntityState.Unchanged;
                Firewall a = ctx.Firewalls.Add(t);

                ctx.SaveChanges();
                return a;
            }
        }

        public Firewall Read(int id)
        {
            using (var ctx = new CADBContext())
            {
                return ctx.Firewalls.Include(m => m.Customer).FirstOrDefault(x => x.Id == id);
            }
        }

        public List<Firewall> ReadAll()
        {
            using (var ctx = new CADBContext())
            {
                var c = ctx.Firewalls.Include(m => m.Customer).ToList();
                return c;
            }
        }

        public bool Delete(Firewall t)
        {
            using (var ctx = new CADBContext())
            {
                if (FirewallExists(t.Id, ctx))
                {
                    ctx.Firewalls.Attach(t);
                    ctx.Firewalls.Remove(t);
                    ctx.SaveChanges();
                    return true;
                }
                return false;


            }
        }

        private bool FirewallExists(int tId, CADBContext ctx)
        {
            if (ctx.Firewalls.Count(a => a.Id == tId) > 0)
            {
                return true;
            }
            return false;

        }

        public bool Update(Firewall t)
        {
            using (var ctx = new CADBContext())
            {
                ctx.Firewalls.Attach(t);
                ctx.Entry(t).State = EntityState.Modified;
                try
                {
                    ctx.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FirewallExists(t.Id, ctx))
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

        public List<Firewall> ReadAllWithFk(int id)
        {
            using (var ctx = new CADBContext())
            {
                var c = ctx.Firewalls.Include(m => m.Customer).Where(x => x.Customer.Id == id).ToList();
                return c;
            }
        }
    }
}
