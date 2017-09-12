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
                Port a = ctx.Ports.Add(t);
                if (t.Swtich != null)
                {
                    ctx.Ports.AddRange(t.Swtich.Ports);
                }
                ctx.SaveChanges();
                return a;
            }
        }

        public Port Read(int id)
        {
            using (var ctx = new CADBContext())
            {
                var a = ctx.Ports.Include(y => y.Swtich).FirstOrDefault(x => x.Id == id);
                return a;
            }
        }

        public List<Port> ReadAll()
        {
            using (var ctx = new CADBContext())
            {
                var a = ctx.Ports.Include(y => y.Swtich).ToList();
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
            if (ctx.Switches.Count(a => a.Id == id) > 0)
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
                var afk = ctx.Ports.Include(y => y.Swtich).ToList();
                return afk;
            }
        }
    }
}
