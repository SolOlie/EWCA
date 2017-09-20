using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DAL.DB;
using Entities.Entities;
using System.Configuration;
using System.Runtime.Remoting.Contexts;

namespace DAL.Repositories
{
    class SwitchRepo : IRepository<Switch>
    {
        public Switch Create(Switch t)
        {
                    using (var ctx = new CADBContext())
                    {

                ctx.Database.Connection.Open();

                t.CustomerId = t.Customer.Id;
                        t.AssetId = t.Asset.Id;

                        ctx.Entry(t.Customer).State = EntityState.Unchanged;
                        ctx.Entry(t.Asset).State = EntityState.Unchanged;

                        Switch a = ctx.Switches.Add(t);
                        if (t.Ports != null)
                        {
                            foreach (var p in t.Ports)
                            {
                                p.SwitchId = a.Id;

                            }
                            ctx.Ports.AddRange(t.Ports);
                        }
                        
                           ctx.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [CustomerAccountingDB].[dbo].[Switches] ON");
                             ctx.SaveChanges();
                        ctx.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [CustomerAccountingDB].[dbo].[Switches] OFF");
                   
                        return a;
                    
                }
               
            
           
        }

        public Switch Read(int id)
        {
            using (var ctx = new CADBContext())
            {
                var a = ctx.Switches.Include(y => y.Ports).Include(c=> c.Customer).Include(e => e.Ports.Select(p=> p.Asset)).FirstOrDefault(x => x.Id == id);
                return a;
            }
        }

        public List<Switch> ReadAll()
        {
            using (var ctx = new CADBContext())
            {
                var a = ctx.Switches.Include(y => y.Ports).Include(c => c.Customer).Include(e => e.Ports.Select(p => p.Asset)).ToList();
                return a;
            }
        }

        public bool Delete(Switch t)
        {
            using (var ctx = new CADBContext())
            {
                if (SwitchExists(t.Id, ctx))
                {
                    ctx.Switches.Attach(t);
                    ctx.Switches.Remove(t);
                    ctx.SaveChanges();
                    return true;
                }
                return false;


            }
        }

        public bool Update(Switch t)
        {
            using (var ctx = new CADBContext())
            {
                t.CustomerId = t.Customer.Id;
                ctx.Switches.Attach(t);
                ctx.Entry(t).State = EntityState.Modified;
                ctx.Entry(t.Ports).State = EntityState.Modified;
                ctx.Entry(t.Customer).State = EntityState.Unchanged;
                try
                {
                    ctx.SaveChanges(t.Id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SwitchExists(t.Id, ctx))
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

        public List<Switch> ReadAllWithFk(int id)
        {
            using (var ctx = new CADBContext())
            {
                var afk = ctx.Switches.Include(y => y.Customer).Include(y => y.Ports).Include(y => y.Ports.Select(p=> p.Asset)).Where(x => x.Customer.Id == id).ToList();
                return afk;
            }
        }

        bool SwitchExists(int id, CADBContext ctx)
        {
            if (ctx.Switches.Count(a => a.Id == id) > 0)
            {
                return true;
            }
            return false;
        }
    }
}
